using Ryujinx.Graphics.Shader.CodeGen.Glsl;
using Ryujinx.Graphics.Shader.CodeGen.Spirv;
using Ryujinx.Graphics.Shader.Decoders;
using Ryujinx.Graphics.Shader.IntermediateRepresentation;
using Ryujinx.Graphics.Shader.StructuredIr;
using Ryujinx.Graphics.Shader.Translation.Optimizations;
using System;
using System.Collections.Generic;

using static Ryujinx.Graphics.Shader.IntermediateRepresentation.OperandHelper;

namespace Ryujinx.Graphics.Shader.Translation
{
    public static class Translator
    {
        private const int HeaderSize = 0x50;

        internal struct FunctionCode
        {
            public Operation[] Code { get; }

            public FunctionCode(Operation[] code)
            {
                Code = code;
            }
        }

        public static TranslatorContext CreateContext(ulong address, IGpuAccessor gpuAccessor, TranslationOptions options)
        {
            Block[][] cfg = DecodeShader(address, gpuAccessor, options, out ShaderConfig config);

            return new TranslatorContext(address, cfg, config);
        }

        internal static ShaderProgram Translate(FunctionCode[] functions, ShaderConfig config, out ShaderProgramInfo shaderProgramInfo)
        {
            var cfgs = new ControlFlowGraph[functions.Length];
            var frus = new RegisterUsage.FunctionRegisterUsage[functions.Length];

            for (int i = 0; i < functions.Length; i++)
            {
                cfgs[i] = ControlFlowGraph.Create(functions[i].Code);

                if (i != 0)
                {
                    frus[i] = RegisterUsage.RunPass(cfgs[i]);
                }
            }

            Function[] funcs = new Function[functions.Length];

            for (int i = 0; i < functions.Length; i++)
            {
                var cfg = cfgs[i];

                int inArgumentsCount = 0;
                int outArgumentsCount = 0;

                if (i != 0)
                {
                    var fru = frus[i];

                    inArgumentsCount  = fru.InArguments.Length;
                    outArgumentsCount = fru.OutArguments.Length;
                }

                if (cfg.Blocks.Length != 0)
                {
                    RegisterUsage.FixupCalls(cfg.Blocks, frus);

                    Dominance.FindDominators(cfg);
                    Dominance.FindDominanceFrontiers(cfg.Blocks);

                    Ssa.Rename(cfg.Blocks);

                    Optimizer.RunPass(cfg.Blocks, config);
                    Rewriter.RunPass(cfg.Blocks, config);
                }

                funcs[i] = new Function(cfg.Blocks, $"fun{i}", false, inArgumentsCount, outArgumentsCount);
            }

            var sInfo = StructuredProgram.MakeStructuredProgram(funcs, config);

            ShaderProgram program = config.Options.TargetLanguage switch
            {
                TargetLanguage.Glsl => new ShaderProgram(config.Stage, GlslGenerator.Generate(sInfo, config)),
                TargetLanguage.Spirv => new ShaderProgram(config.Stage, SpirvGenerator.Generate(sInfo, config)),
                _ => throw new NotImplementedException(config.Options.TargetLanguage.ToString())
            };

            shaderProgramInfo = new ShaderProgramInfo(
                config.GetConstantBufferDescriptors(),
                config.GetStorageBufferDescriptors(),
                config.GetTextureDescriptors(),
                config.GetImageDescriptors(),
                config.UsedFeatures.HasFlag(FeatureFlags.InstanceId),
                config.ClipDistancesWritten);

            return program;
        }

        private static Block[][] DecodeShader(
            ulong address,
            IGpuAccessor gpuAccessor,
            TranslationOptions options,
            out ShaderConfig config)
        {
            Block[][] cfg;
            ulong maxEndAddress = 0;

            bool hasBindless;

            if (options.Flags.HasFlag(TranslationFlags.Compute))
            {
                config = new ShaderConfig(gpuAccessor, options);

                cfg = Decoder.Decode(gpuAccessor, address, out hasBindless);
            }
            else
            {
                config = new ShaderConfig(new ShaderHeader(gpuAccessor, address), gpuAccessor, options);

                cfg = Decoder.Decode(gpuAccessor, address + HeaderSize, out hasBindless);
            }

            if (hasBindless)
            {
                config.SetUsedFeature(FeatureFlags.Bindless);
            }

            for (int funcIndex = 0; funcIndex < cfg.Length; funcIndex++)
            {
                for (int blkIndex = 0; blkIndex < cfg[funcIndex].Length; blkIndex++)
                {
                    Block block = cfg[funcIndex][blkIndex];

                    if (maxEndAddress < block.EndAddress)
                    {
                        maxEndAddress = block.EndAddress;
                    }

                    if (!hasBindless)
                    {
                        for (int index = 0; index < block.OpCodes.Count; index++)
                        {
                            if (block.OpCodes[index] is OpCodeTextureBase texture)
                            {
                                config.TextureHandlesForCache.Add(texture.HandleOffset);
                            }
                        }
                    }
                }
            }

            config.SizeAdd((int)maxEndAddress + (options.Flags.HasFlag(TranslationFlags.Compute) ? 0 : HeaderSize));

            return cfg;
        }

        internal static FunctionCode[] EmitShader(Block[][] cfg, ShaderConfig config)
        {
            Dictionary<ulong, int> funcIds = new Dictionary<ulong, int>();

            for (int funcIndex = 0; funcIndex < cfg.Length; funcIndex++)
            {
                funcIds.Add(cfg[funcIndex][0].Address, funcIndex);
            }

            List<FunctionCode> funcs = new List<FunctionCode>();

            for (int funcIndex = 0; funcIndex < cfg.Length; funcIndex++)
            {
                EmitterContext context = new EmitterContext(config, funcIndex != 0, funcIds);

                for (int blkIndex = 0; blkIndex < cfg[funcIndex].Length; blkIndex++)
                {
                    Block block = cfg[funcIndex][blkIndex];

                    context.CurrBlock = block;

                    context.MarkLabel(context.GetLabel(block.Address));

                    EmitOps(context, block);
                }

                funcs.Add(new FunctionCode(context.GetOperations()));
            }

            return funcs.ToArray();
        }

        private static void EmitOps(EmitterContext context, Block block)
        {
            for (int opIndex = 0; opIndex < block.OpCodes.Count; opIndex++)
            {
                OpCode op = block.OpCodes[opIndex];

                if ((context.Config.Options.Flags & TranslationFlags.DebugMode) != 0)
                {
                    string instName;

                    if (op.Emitter != null)
                    {
                        instName = op.Emitter.Method.Name;
                    }
                    else
                    {
                        instName = "???";

                        context.Config.GpuAccessor.Log($"Invalid instruction at 0x{op.Address:X6} (0x{op.RawOpCode:X16}).");
                    }

                    string dbgComment = $"0x{op.Address:X6}: 0x{op.RawOpCode:X16} {instName}";

                    context.Add(new CommentNode(dbgComment));
                }

                if (op.NeverExecute)
                {
                    continue;
                }

                Operand predSkipLbl = null;

                bool skipPredicateCheck = op is OpCodeBranch opBranch && !opBranch.PushTarget;

                if (op is OpCodeBranchPop opBranchPop)
                {
                    // If the instruction is a SYNC or BRK instruction with only one
                    // possible target address, then the instruction is basically
                    // just a simple branch, we can generate code similar to branch
                    // instructions, with the condition check on the branch itself.
                    skipPredicateCheck = opBranchPop.Targets.Count < 2;
                }

                if (!(op.Predicate.IsPT || skipPredicateCheck))
                {
                    Operand label;

                    if (opIndex == block.OpCodes.Count - 1 && block.Next != null)
                    {
                        label = context.GetLabel(block.Next.Address);
                    }
                    else
                    {
                        label = Label();

                        predSkipLbl = label;
                    }

                    Operand pred = Register(op.Predicate);

                    if (op.InvertPredicate)
                    {
                        context.BranchIfTrue(label, pred);
                    }
                    else
                    {
                        context.BranchIfFalse(label, pred);
                    }
                }

                context.CurrOp = op;

                op.Emitter?.Invoke(context);

                if (predSkipLbl != null)
                {
                    context.MarkLabel(predSkipLbl);
                }
            }
        }
    }
}