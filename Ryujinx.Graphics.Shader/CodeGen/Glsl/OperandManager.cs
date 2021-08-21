using Ryujinx.Graphics.Shader.IntermediateRepresentation;
using Ryujinx.Graphics.Shader.StructuredIr;
using Ryujinx.Graphics.Shader.Translation;
using System;
using System.Collections.Generic;
using System.Diagnostics;

using static Ryujinx.Graphics.Shader.StructuredIr.InstructionInfo;

namespace Ryujinx.Graphics.Shader.CodeGen.Glsl
{
    class OperandManager
    {
        private static readonly string[] StagePrefixes = new string[] { "cp", "vp", "tcp", "tep", "gp", "fp" };

        private struct BuiltInAttribute
        {
            public string Name { get; }

            public VariableType Type { get; }

            public BuiltInAttribute(string name, VariableType type)
            {
                Name = name;
                Type = type;
            }
        }

        private static Dictionary<int, BuiltInAttribute> BuiltInAttributes = new Dictionary<int, BuiltInAttribute>()
        {
            { AttributeConsts.Layer,               new BuiltInAttribute("gl_Layer",           VariableType.S32)  },
            { AttributeConsts.PointSize,           new BuiltInAttribute("gl_PointSize",       VariableType.F32)  },
            { AttributeConsts.PositionX,           new BuiltInAttribute("gl_Position.x",      VariableType.F32)  },
            { AttributeConsts.PositionY,           new BuiltInAttribute("gl_Position.y",      VariableType.F32)  },
            { AttributeConsts.PositionZ,           new BuiltInAttribute("gl_Position.z",      VariableType.F32)  },
            { AttributeConsts.PositionW,           new BuiltInAttribute("gl_Position.w",      VariableType.F32)  },
            { AttributeConsts.ClipDistance0,       new BuiltInAttribute("gl_ClipDistance[0]", VariableType.F32)  },
            { AttributeConsts.ClipDistance1,       new BuiltInAttribute("gl_ClipDistance[1]", VariableType.F32)  },
            { AttributeConsts.ClipDistance2,       new BuiltInAttribute("gl_ClipDistance[2]", VariableType.F32)  },
            { AttributeConsts.ClipDistance3,       new BuiltInAttribute("gl_ClipDistance[3]", VariableType.F32)  },
            { AttributeConsts.ClipDistance4,       new BuiltInAttribute("gl_ClipDistance[4]", VariableType.F32)  },
            { AttributeConsts.ClipDistance5,       new BuiltInAttribute("gl_ClipDistance[5]", VariableType.F32)  },
            { AttributeConsts.ClipDistance6,       new BuiltInAttribute("gl_ClipDistance[6]", VariableType.F32)  },
            { AttributeConsts.ClipDistance7,       new BuiltInAttribute("gl_ClipDistance[7]", VariableType.F32)  },
            { AttributeConsts.PointCoordX,         new BuiltInAttribute("gl_PointCoord.x",    VariableType.F32)  },
            { AttributeConsts.PointCoordY,         new BuiltInAttribute("gl_PointCoord.y",    VariableType.F32)  },
            { AttributeConsts.TessCoordX,          new BuiltInAttribute("gl_TessCoord.x",     VariableType.F32)  },
            { AttributeConsts.TessCoordY,          new BuiltInAttribute("gl_TessCoord.y",     VariableType.F32)  },
            { AttributeConsts.InstanceId,          new BuiltInAttribute("gl_InstanceID",      VariableType.S32)  },
            { AttributeConsts.VertexId,            new BuiltInAttribute("gl_VertexID",        VariableType.S32)  },
            { AttributeConsts.FrontFacing,         new BuiltInAttribute("gl_FrontFacing",     VariableType.Bool) },

            // Special.
            { AttributeConsts.FragmentOutputDepth, new BuiltInAttribute("gl_FragDepth",                           VariableType.F32)  },
            { AttributeConsts.ThreadKill,          new BuiltInAttribute("gl_HelperInvocation",                    VariableType.Bool) },
            { AttributeConsts.ThreadIdX,           new BuiltInAttribute("gl_LocalInvocationID.x",                 VariableType.U32)  },
            { AttributeConsts.ThreadIdY,           new BuiltInAttribute("gl_LocalInvocationID.y",                 VariableType.U32)  },
            { AttributeConsts.ThreadIdZ,           new BuiltInAttribute("gl_LocalInvocationID.z",                 VariableType.U32)  },
            { AttributeConsts.CtaIdX,              new BuiltInAttribute("gl_WorkGroupID.x",                       VariableType.U32)  },
            { AttributeConsts.CtaIdY,              new BuiltInAttribute("gl_WorkGroupID.y",                       VariableType.U32)  },
            { AttributeConsts.CtaIdZ,              new BuiltInAttribute("gl_WorkGroupID.z",                       VariableType.U32)  },
            { AttributeConsts.LaneId,              new BuiltInAttribute("gl_SubGroupInvocationARB",               VariableType.U32)  },
            { AttributeConsts.EqMask,              new BuiltInAttribute("unpackUint2x32(gl_SubGroupEqMaskARB).x", VariableType.U32)  },
            { AttributeConsts.GeMask,              new BuiltInAttribute("unpackUint2x32(gl_SubGroupGeMaskARB).x", VariableType.U32)  },
            { AttributeConsts.GtMask,              new BuiltInAttribute("unpackUint2x32(gl_SubGroupGtMaskARB).x", VariableType.U32)  },
            { AttributeConsts.LeMask,              new BuiltInAttribute("unpackUint2x32(gl_SubGroupLeMaskARB).x", VariableType.U32)  },
            { AttributeConsts.LtMask,              new BuiltInAttribute("unpackUint2x32(gl_SubGroupLtMaskARB).x", VariableType.U32)  },

            // Support uniforms.
            { AttributeConsts.FragmentOutputIsBgraBase + 0,  new BuiltInAttribute($"{DefaultNames.SupportBlockIsBgraName}[0]",  VariableType.Bool) },
            { AttributeConsts.FragmentOutputIsBgraBase + 4,  new BuiltInAttribute($"{DefaultNames.SupportBlockIsBgraName}[1]",  VariableType.Bool) },
            { AttributeConsts.FragmentOutputIsBgraBase + 8,  new BuiltInAttribute($"{DefaultNames.SupportBlockIsBgraName}[2]",  VariableType.Bool) },
            { AttributeConsts.FragmentOutputIsBgraBase + 12, new BuiltInAttribute($"{DefaultNames.SupportBlockIsBgraName}[3]",  VariableType.Bool) },
            { AttributeConsts.FragmentOutputIsBgraBase + 16, new BuiltInAttribute($"{DefaultNames.SupportBlockIsBgraName}[4]",  VariableType.Bool) },
            { AttributeConsts.FragmentOutputIsBgraBase + 20, new BuiltInAttribute($"{DefaultNames.SupportBlockIsBgraName}[5]",  VariableType.Bool) },
            { AttributeConsts.FragmentOutputIsBgraBase + 24, new BuiltInAttribute($"{DefaultNames.SupportBlockIsBgraName}[6]",  VariableType.Bool) },
            { AttributeConsts.FragmentOutputIsBgraBase + 28, new BuiltInAttribute($"{DefaultNames.SupportBlockIsBgraName}[7]",  VariableType.Bool) }
        };

        private Dictionary<AstOperand, string> _locals;

        public OperandManager()
        {
            _locals = new Dictionary<AstOperand, string>();
        }

        public string DeclareLocal(AstOperand operand)
        {
            string name = $"{DefaultNames.LocalNamePrefix}_{_locals.Count}";

            _locals.Add(operand, name);

            return name;
        }

        public string GetExpression(AstOperand operand, ShaderConfig config)
        {
            return operand.Type switch
            {
                OperandType.Argument => GetArgumentName(operand.Value),
                OperandType.Attribute => GetAttributeName(operand, config),
                OperandType.Constant => NumberFormatter.FormatInt(operand.Value),
                OperandType.ConstantBuffer => GetConstantBufferName(
                    operand.CbufSlot,
                    operand.CbufOffset,
                    config.Stage,
                    config.UsedFeatures.HasFlag(FeatureFlags.CbIndexing)),
                OperandType.LocalVariable => _locals[operand],
                OperandType.Undefined => DefaultNames.UndefinedName,
                _ => throw new ArgumentException($"Invalid operand type \"{operand.Type}\".")
            };
        }

        public static string GetConstantBufferName(int slot, int offset, ShaderStage stage, bool cbIndexable)
        {
            return $"{GetUbName(stage, slot, cbIndexable)}[{offset >> 2}].{GetSwizzleMask(offset & 3)}";
        }

        private static string GetVec4Indexed(string vectorName, string indexExpr)
        {
            string result = $"{vectorName}.x";
            for (int i = 1; i < 4; i++)
            {
                result = $"(({indexExpr}) == {i}) ? ({vectorName}.{GetSwizzleMask(i)}) : ({result})";
            }
            return $"({result})";
        }

        public static string GetConstantBufferName(int slot, string offsetExpr, ShaderStage stage, bool cbIndexable)
        {
            return GetVec4Indexed(GetUbName(stage, slot, cbIndexable) + $"[{offsetExpr} >> 2]", offsetExpr + " & 3");
        }

        public static string GetConstantBufferName(string slotExpr, string offsetExpr, ShaderStage stage)
        {
            return GetVec4Indexed(GetUbName(stage, slotExpr) + $"[{offsetExpr} >> 2]", offsetExpr + " & 3");
        }

        public static string GetOutAttributeName(AstOperand attr, ShaderConfig config)
        {
            return GetAttributeName(attr, config, isOutAttr: true);
        }

        public static string GetAttributeName(AstOperand attr, ShaderConfig config, bool isOutAttr = false, string indexExpr = "0")
        {
            int value = attr.Value;

            char swzMask = GetSwizzleMask((value >> 2) & 3);

            if (value >= AttributeConsts.UserAttributeBase && value < AttributeConsts.UserAttributeEnd)
            {
                value -= AttributeConsts.UserAttributeBase;

                string prefix = isOutAttr
                    ? DefaultNames.OAttributePrefix
                    : DefaultNames.IAttributePrefix;

                if (config.Options.Flags.HasFlag(TranslationFlags.Feedback))
                {
                    string name = $"{prefix}{(value >> 4)}_{swzMask}";

                    if (config.Stage == ShaderStage.Geometry && !isOutAttr)
                    {
                        name += $"[{indexExpr}]";
                    }

                    return name;
                }
                else
                {
                    string name = $"{prefix}{(value >> 4)}";

                    if (config.Stage == ShaderStage.Geometry && !isOutAttr)
                    {
                        name += $"[{indexExpr}]";
                    }

                    return name + '.' + swzMask;
                }
            }
            else
            {
                if (value >= AttributeConsts.FragmentOutputColorBase && value < AttributeConsts.FragmentOutputColorEnd)
                {
                    value -= AttributeConsts.FragmentOutputColorBase;

                    return $"{DefaultNames.OAttributePrefix}{(value >> 4)}.{swzMask}";
                }
                else if (BuiltInAttributes.TryGetValue(value & ~3, out BuiltInAttribute builtInAttr))
                {
                    // TODO: There must be a better way to handle this...
                    if (config.Stage == ShaderStage.Fragment)
                    {
                        switch (value & ~3)
                        {
                            case AttributeConsts.PositionX: return $"(gl_FragCoord.x / {DefaultNames.SupportBlockRenderScaleName}[0])";
                            case AttributeConsts.PositionY: return $"(gl_FragCoord.y / {DefaultNames.SupportBlockRenderScaleName}[0])";
                            case AttributeConsts.PositionZ: return "gl_FragCoord.z";
                            case AttributeConsts.PositionW: return "gl_FragCoord.w";
                        }
                    }

                    string name = builtInAttr.Name;

                    if (config.Stage == ShaderStage.Geometry && (value & AttributeConsts.SpecialMask) == 0 && !isOutAttr)
                    {
                        name = $"gl_in[{indexExpr}].{name}";
                    }

                    return name;
                }
            }

            // TODO: Warn about unknown built-in attribute.

            return isOutAttr ? "// bad_attr0x" + value.ToString("X") : "0.0";
        }

        public static string GetUbName(ShaderStage stage, int slot, bool cbIndexable)
        {
            if (cbIndexable)
            {
                return GetUbName(stage, NumberFormatter.FormatInt(slot, VariableType.S32));
            }

            return $"{GetShaderStagePrefix(stage)}_{DefaultNames.UniformNamePrefix}{slot}_{DefaultNames.UniformNameSuffix}";
        }

        private static string GetUbName(ShaderStage stage, string slotExpr)
        {
            return $"{GetShaderStagePrefix(stage)}_{DefaultNames.UniformNamePrefix}[{slotExpr}].{DefaultNames.DataName}";
        }

        public static string GetSamplerName(ShaderStage stage, AstTextureOperation texOp, string indexExpr)
        {
            return GetSamplerName(stage, texOp.CbufSlot, texOp.Handle, texOp.Type.HasFlag(SamplerType.Indexed), indexExpr);
        }

        public static string GetSamplerName(ShaderStage stage, int cbufSlot, int handle, bool indexed, string indexExpr)
        {
            string suffix = cbufSlot < 0 ? $"_tcb_{handle:X}" : $"_cb{cbufSlot}_{handle:X}";

            if (indexed)
            {
                suffix += $"a[{indexExpr}]";
            }

            return GetShaderStagePrefix(stage) + "_" + DefaultNames.SamplerNamePrefix + suffix;
        }

        public static string GetImageName(ShaderStage stage, AstTextureOperation texOp, string indexExpr)
        {
            return GetImageName(stage, texOp.CbufSlot, texOp.Handle, texOp.Format, texOp.Type.HasFlag(SamplerType.Indexed), indexExpr);
        }

        public static string GetImageName(
            ShaderStage stage,
            int cbufSlot,
            int handle,
            TextureFormat format,
            bool indexed,
            string indexExpr)
        {
            string suffix = cbufSlot < 0
                ? $"_tcb_{handle:X}_{format.ToGlslFormat()}"
                : $"_cb{cbufSlot}_{handle:X}_{format.ToGlslFormat()}";

            if (indexed)
            {
                suffix += $"a[{indexExpr}]";
            }

            return GetShaderStagePrefix(stage) + "_" + DefaultNames.ImageNamePrefix + suffix;
        }

        public static string GetShaderStagePrefix(ShaderStage stage)
        {
            int index = (int)stage;

            if ((uint)index >= StagePrefixes.Length)
            {
                return "invalid";
            }

            return StagePrefixes[index];
        }

        private static char GetSwizzleMask(int value)
        {
            return "xyzw"[value];
        }

        public static string GetArgumentName(int argIndex)
        {
            return $"{DefaultNames.ArgumentNamePrefix}{argIndex}";
        }

        public static VariableType GetNodeDestType(CodeGenContext context, IAstNode node, bool isAsgDest = false)
        {
            if (node is AstOperation operation)
            {
                // Load attribute basically just returns the attribute value.
                // Some built-in attributes may have different types, so we need
                // to return the type based on the attribute that is being read.
                if (operation.Inst == Instruction.LoadAttribute)
                {
                    return GetOperandVarType(context, (AstOperand)operation.GetSource(0));
                }
                else if (operation.Inst == Instruction.Call)
                {
                    AstOperand funcId = (AstOperand)operation.GetSource(0);

                    Debug.Assert(funcId.Type == OperandType.Constant);

                    return context.GetFunction(funcId.Value).ReturnType;
                }
                else if (operation is AstTextureOperation texOp &&
                         (texOp.Inst == Instruction.ImageLoad ||
                          texOp.Inst == Instruction.ImageStore))
                {
                    return texOp.Format.GetComponentType();
                }

                return GetDestVarType(operation.Inst);
            }
            else if (node is AstOperand operand)
            {
                if (operand.Type == OperandType.Argument)
                {
                    int argIndex = operand.Value;

                    return context.CurrentFunction.GetArgumentType(argIndex);
                }

                return GetOperandVarType(context, operand, isAsgDest);
            }
            else
            {
                throw new ArgumentException($"Invalid node type \"{node?.GetType().Name ?? "null"}\".");
            }
        }

        private static VariableType GetOperandVarType(CodeGenContext context, AstOperand operand, bool isAsgDest = false)
        {
            if (operand.Type == OperandType.Attribute)
            {
                if (BuiltInAttributes.TryGetValue(operand.Value & ~3, out BuiltInAttribute builtInAttr))
                {
                    return builtInAttr.Type;
                }
                else if (context.Config.Stage == ShaderStage.Vertex && !isAsgDest &&
                    operand.Value >= AttributeConsts.UserAttributeBase &&
                    operand.Value < AttributeConsts.UserAttributeEnd)
                {
                    int location = (operand.Value - AttributeConsts.UserAttributeBase) / 16;

                    AttributeType type = context.Config.GpuAccessor.QueryAttributeType(location);

                    return type switch
                    {
                        AttributeType.Sint => VariableType.S32,
                        AttributeType.Uint => VariableType.U32,
                        _ => VariableType.F32
                    };
                }
            }

            return OperandInfo.GetVarType(operand);
        }
    }
}