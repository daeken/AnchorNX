using Ryujinx.Graphics.Shader.Decoders;
using Ryujinx.Graphics.Shader.IntermediateRepresentation;
using System.Collections.Generic;

using static Ryujinx.Graphics.Shader.IntermediateRepresentation.OperandHelper;

namespace Ryujinx.Graphics.Shader.Translation
{
    class EmitterContext
    {
        public Block  CurrBlock { get; set; }
        public OpCode CurrOp    { get; set; }

        public ShaderConfig Config { get; }

        public bool IsNonMain { get; }

        private readonly IReadOnlyDictionary<ulong, int> _funcs;
        private readonly List<Operation> _operations;
        private readonly Dictionary<ulong, Operand> _labels;

        public EmitterContext(ShaderConfig config, bool isNonMain, IReadOnlyDictionary<ulong, int> funcs)
        {
            Config = config;
            IsNonMain = isNonMain;
            _funcs = funcs;
            _operations = new List<Operation>();
            _labels = new Dictionary<ulong, Operand>();

            EmitStart();
        }

        private void EmitStart()
        {
            if (Config.Stage == ShaderStage.Vertex &&
                Config.Options.TargetApi == TargetApi.Vulkan &&
                (Config.Options.Flags & TranslationFlags.VertexA) == 0)
            {
                // Vulkan requires the point size to be always written on the shader if the primitive topology is points.
                this.Copy(Attribute(AttributeConsts.PointSize), ConstF(Config.GpuAccessor.QueryPointSize()));
            }
        }

        public Operand Add(Instruction inst, Operand dest = null, params Operand[] sources)
        {
            Operation operation = new Operation(inst, dest, sources);

            _operations.Add(operation);

            return dest;
        }

        public (Operand, Operand) Add(Instruction inst, (Operand, Operand) dest, params Operand[] sources)
        {
            Operand[] dests = new[] { dest.Item1, dest.Item2 };

            Operation operation = new Operation(inst, 0, dests, sources);

            Add(operation);

            return dest;
        }

        public void Add(Operation operation)
        {
            _operations.Add(operation);
        }

        public TextureOperation CreateTextureOperation(
            Instruction inst,
            SamplerType type,
            TextureFlags flags,
            int handle,
            int compIndex,
            Operand dest,
            params Operand[] sources)
        {
            return CreateTextureOperation(inst, type, TextureFormat.Unknown, flags, handle, compIndex, dest, sources);
        }

        public TextureOperation CreateTextureOperation(
            Instruction inst,
            SamplerType type,
            TextureFormat format,
            TextureFlags flags,
            int handle,
            int compIndex,
            Operand dest,
            params Operand[] sources)
        {
            if (!flags.HasFlag(TextureFlags.Bindless))
            {
                Config.SetUsedTexture(inst, type, format, flags, TextureOperation.DefaultCbufSlot, handle);
            }

            return new TextureOperation(inst, type, format, flags, handle, compIndex, dest, sources);
        }

        public void FlagAttributeRead(int attribute)
        {
            if (Config.Stage == ShaderStage.Vertex && attribute == AttributeConsts.InstanceId)
            {
                Config.SetUsedFeature(FeatureFlags.InstanceId);
            }
            else if (Config.Stage == ShaderStage.Fragment)
            {
                switch (attribute)
                {
                    case AttributeConsts.PositionX:
                    case AttributeConsts.PositionY:
                        Config.SetUsedFeature(FeatureFlags.FragCoordXY);
                        break;
                }
            }
        }

        public void FlagAttributeWritten(int attribute)
        {
            if (Config.Stage == ShaderStage.Vertex)
            {
                switch (attribute)
                {
                    case AttributeConsts.ClipDistance0:
                    case AttributeConsts.ClipDistance1:
                    case AttributeConsts.ClipDistance2:
                    case AttributeConsts.ClipDistance3:
                    case AttributeConsts.ClipDistance4:
                    case AttributeConsts.ClipDistance5:
                    case AttributeConsts.ClipDistance6:
                    case AttributeConsts.ClipDistance7:
                        Config.SetClipDistanceWritten((attribute - AttributeConsts.ClipDistance0) / 4);
                        break;
                }
            }
        }

        public void MarkLabel(Operand label)
        {
            Add(Instruction.MarkLabel, label);
        }

        public Operand GetLabel(ulong address)
        {
            if (!_labels.TryGetValue(address, out Operand label))
            {
                label = Label();

                _labels.Add(address, label);
            }

            return label;
        }

        public int GetFunctionId(ulong address)
        {
            return _funcs[address];
        }

        public void PrepareForReturn()
        {
            if (IsNonMain)
            {
                return;
            }

            if (Config.Options.TargetApi == TargetApi.Vulkan &&
                Config.Stage == ShaderStage.Vertex &&
                (Config.Options.Flags & TranslationFlags.VertexA) == 0)
            {
                if (Config.GpuAccessor.QueryTransformDepthMinusOneToOne())
                {
                    Operand z = Attribute(AttributeConsts.PositionZ);
                    Operand w = Attribute(AttributeConsts.PositionW);
                    Operand halfW = this.FPMultiply(w, ConstF(0.5f));

                    this.Copy(Attribute(AttributeConsts.PositionZ), this.FPFusedMultiplyAdd(z, ConstF(0.5f), halfW));
                }
            }
            else if (Config.Stage == ShaderStage.Fragment)
            {
                if (Config.OmapDepth)
                {
                    Operand dest = Attribute(AttributeConsts.FragmentOutputDepth);

                    Operand src = Register(Config.GetDepthRegister(), RegisterType.Gpr);

                    this.Copy(dest, src);
                }

                int regIndexBase = 0;

                for (int rtIndex = 0; rtIndex < 8; rtIndex++)
                {
                    OmapTarget target = Config.OmapTargets[rtIndex];

                    for (int component = 0; component < 4; component++)
                    {
                        if (!target.ComponentEnabled(component))
                        {
                            continue;
                        }

                        int fragmentOutputColorAttr = AttributeConsts.FragmentOutputColorBase + rtIndex * 16;

                        Operand src = Register(regIndexBase + component, RegisterType.Gpr);

                        // Perform B <-> R swap if needed, for BGRA formats (not supported on OpenGL).
                        /* if (component == 0 || component == 2)
                        {
                            Operand isBgra = Config.AddInput(AttributeConsts.FragmentOutputIsBgraBase + rtIndex * 4);

                            Operand lblIsBgra = Label();
                            Operand lblEnd    = Label();

                            this.BranchIfTrue(lblIsBgra, isBgra);

                            this.Copy(Config.AddOutput(fragmentOutputColorAttr + component * 4), src);
                            this.Branch(lblEnd);

                            MarkLabel(lblIsBgra);

                            this.Copy(Config.AddOutput(fragmentOutputColorAttr + (2 - component) * 4), src);

                            MarkLabel(lblEnd);
                        }
                        else */
                        {
                            this.Copy(Attribute(fragmentOutputColorAttr + component * 4), src);
                        }
                    }

                    if (target.Enabled)
                    {
                        regIndexBase += 4;
                    }
                }
            }
        }

        public Operation[] GetOperations()
        {
            return _operations.ToArray();
        }
    }
}