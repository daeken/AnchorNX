using Ryujinx.Common;
using Ryujinx.Graphics.Shader.StructuredIr;
using Ryujinx.Graphics.Shader.Translation;
using System;
using System.Linq;

namespace Ryujinx.Graphics.Shader.CodeGen.Glsl
{
    static class Declarations
    {
        // At least 16 attributes are guaranteed by the spec.
        public const int MaxAttributes = 16;

        public static void Declare(CodeGenContext context, StructuredProgramInfo info)
        {
            context.AppendLine(context.Config.Options.TargetApi == TargetApi.Vulkan ? "#version 460 core" : "#version 450 core");
            context.AppendLine("#extension GL_ARB_gpu_shader_int64 : enable");
            context.AppendLine("#extension GL_ARB_shader_ballot : enable");
            context.AppendLine("#extension GL_ARB_shader_group_vote : enable");
            context.AppendLine("#extension GL_EXT_shader_image_load_formatted : enable");
            context.AppendLine("#extension GL_EXT_texture_shadow_lod : enable");

            if (context.Config.Stage == ShaderStage.Compute)
            {
                context.AppendLine("#extension GL_ARB_compute_shader : enable");
            }

            if (context.Config.GpPassthrough)
            {
                context.AppendLine("#extension GL_NV_geometry_shader_passthrough : enable");
            }

            context.AppendLine("#pragma optionNV(fastmath off)");
            context.AppendLine();

            context.AppendLine($"const int {DefaultNames.UndefinedName} = 0;");
            context.AppendLine();

            if (context.Config.Stage == ShaderStage.Compute)
            {
                int localMemorySize = BitUtils.DivRoundUp(context.Config.GpuAccessor.QueryComputeLocalMemorySize(), 4);

                if (localMemorySize != 0)
                {
                    string localMemorySizeStr = NumberFormatter.FormatInt(localMemorySize);

                    context.AppendLine($"uint {DefaultNames.LocalMemoryName}[{localMemorySizeStr}];");
                    context.AppendLine();
                }

                int sharedMemorySize = BitUtils.DivRoundUp(context.Config.GpuAccessor.QueryComputeSharedMemorySize(), 4);

                if (sharedMemorySize != 0)
                {
                    string sharedMemorySizeStr = NumberFormatter.FormatInt(sharedMemorySize);

                    context.AppendLine($"shared uint {DefaultNames.SharedMemoryName}[{sharedMemorySizeStr}];");
                    context.AppendLine();
                }
            }
            else if (context.Config.LocalMemorySize != 0)
            {
                int localMemorySize = BitUtils.DivRoundUp(context.Config.LocalMemorySize, 4);

                string localMemorySizeStr = NumberFormatter.FormatInt(localMemorySize);

                context.AppendLine($"uint {DefaultNames.LocalMemoryName}[{localMemorySizeStr}];");
                context.AppendLine();
            }

            var cBufferDescriptors = context.Config.GetConstantBufferDescriptors();
            if (cBufferDescriptors.Length != 0)
            {
                DeclareUniforms(context, cBufferDescriptors);

                context.AppendLine();
            }

            var sBufferDescriptors = context.Config.GetStorageBufferDescriptors();
            if (sBufferDescriptors.Length != 0)
            {
                DeclareStorages(context, sBufferDescriptors);

                context.AppendLine();
            }

            var textureDescriptors = context.Config.GetTextureDescriptors();
            if (textureDescriptors.Length != 0)
            {
                DeclareSamplers(context, textureDescriptors);

                context.AppendLine();
            }

            var imageDescriptors = context.Config.GetImageDescriptors();
            if (imageDescriptors.Length != 0)
            {
                DeclareImages(context, imageDescriptors);

                context.AppendLine();
            }

            if (context.Config.Stage != ShaderStage.Compute)
            {
                if (context.Config.Stage == ShaderStage.Geometry)
                {
                    string inPrimitive = context.Config.GpuAccessor.QueryPrimitiveTopology().ToGlslString();

                    context.AppendLine($"layout ({inPrimitive}) in;");

                    if (context.Config.GpPassthrough)
                    {
                        context.AppendLine($"layout (passthrough) in gl_PerVertex");
                        context.EnterScope();
                        context.AppendLine("vec4 gl_Position;");
                        context.AppendLine("float gl_PointSize;");
                        context.AppendLine("float gl_ClipDistance[];");
                        context.LeaveScope(";");
                    }
                    else
                    {
                        string outPrimitive = context.Config.OutputTopology.ToGlslString();

                        int maxOutputVertices = context.Config.MaxOutputVertices;

                        context.AppendLine($"layout ({outPrimitive}, max_vertices = {maxOutputVertices}) out;");
                    }

                    context.AppendLine();
                }

                if (info.IAttributes.Count != 0 || context.Config.GpPassthrough)
                {
                    DeclareInputAttributes(context, info);

                    context.AppendLine();
                }

                if (info.OAttributes.Count != 0 || context.Config.Stage != ShaderStage.Fragment)
                {
                    DeclareOutputAttributes(context, info);

                    context.AppendLine();
                }

                if (context.Config.Stage != ShaderStage.Compute &&
                    context.Config.Stage != ShaderStage.Fragment &&
                    (context.Config.Options.Flags & TranslationFlags.Feedback) != 0)
                {
                    var tfOutput = context.GetTransformFeedbackOutput(AttributeConsts.PositionX);
                    if (tfOutput.Valid)
                    {
                        context.AppendLine($"layout (xfb_buffer = {tfOutput.Buffer}, xfb_offset = {tfOutput.Offset}, xfb_stride = {tfOutput.Stride}) out gl_PerVertex");
                        context.EnterScope();
                        context.AppendLine("vec4 gl_Position;");
                        context.LeaveScope(";");
                    }
                }
            }
            else
            {
                string localSizeX = NumberFormatter.FormatInt(context.Config.GpuAccessor.QueryComputeLocalSizeX());
                string localSizeY = NumberFormatter.FormatInt(context.Config.GpuAccessor.QueryComputeLocalSizeY());
                string localSizeZ = NumberFormatter.FormatInt(context.Config.GpuAccessor.QueryComputeLocalSizeZ());

                context.AppendLine(
                    "layout (" +
                    $"local_size_x = {localSizeX}, " +
                    $"local_size_y = {localSizeY}, " +
                    $"local_size_z = {localSizeZ}) in;");
                context.AppendLine();
            }

            bool isFragment = context.Config.Stage == ShaderStage.Fragment;

            if (isFragment || context.Config.Stage == ShaderStage.Compute)
            {
                if (isFragment && context.Config.GpuAccessor.QueryEarlyZForce())
                {
                    context.AppendLine("layout(early_fragment_tests) in;");
                    context.AppendLine();
                }

                if ((context.Config.UsedFeatures & (FeatureFlags.FragCoordXY | FeatureFlags.IntegerSampling)) != 0)
                {
                    string stage = OperandManager.GetShaderStagePrefix(context.Config.Stage);

                    int scaleElements = context.Config.GetTextureDescriptors().Length + context.Config.GetImageDescriptors().Length;

                    if (isFragment)
                    {
                        scaleElements++; // Also includes render target scale, for gl_FragCoord.
                    }

                    DeclareSupportUniformBlock(context, isFragment, scaleElements);

                    if (context.Config.UsedFeatures.HasFlag(FeatureFlags.IntegerSampling))
                    {
                        AppendHelperFunction(context, $"Ryujinx.Graphics.Shader/CodeGen/Glsl/HelperFunctions/TexelFetchScale_{stage}.glsl");
                        context.AppendLine();
                    }
                }
                else if (isFragment)
                {
                    DeclareSupportUniformBlock(context, true, 0);
                }
            }

            if ((info.HelperFunctionsMask & HelperFunctionsMask.AtomicMinMaxS32Shared) != 0)
            {
                AppendHelperFunction(context, "Ryujinx.Graphics.Shader/CodeGen/Glsl/HelperFunctions/AtomicMinMaxS32Shared.glsl");
            }

            if ((info.HelperFunctionsMask & HelperFunctionsMask.AtomicMinMaxS32Storage) != 0)
            {
                AppendHelperFunction(context, "Ryujinx.Graphics.Shader/CodeGen/Glsl/HelperFunctions/AtomicMinMaxS32Storage.glsl");
            }

            if ((info.HelperFunctionsMask & HelperFunctionsMask.MultiplyHighS32) != 0)
            {
                AppendHelperFunction(context, "Ryujinx.Graphics.Shader/CodeGen/Glsl/HelperFunctions/MultiplyHighS32.glsl");
            }

            if ((info.HelperFunctionsMask & HelperFunctionsMask.MultiplyHighU32) != 0)
            {
                AppendHelperFunction(context, "Ryujinx.Graphics.Shader/CodeGen/Glsl/HelperFunctions/MultiplyHighU32.glsl");
            }

            if ((info.HelperFunctionsMask & HelperFunctionsMask.Shuffle) != 0)
            {
                AppendHelperFunction(context, "Ryujinx.Graphics.Shader/CodeGen/Glsl/HelperFunctions/Shuffle.glsl");
            }

            if ((info.HelperFunctionsMask & HelperFunctionsMask.ShuffleDown) != 0)
            {
                AppendHelperFunction(context, "Ryujinx.Graphics.Shader/CodeGen/Glsl/HelperFunctions/ShuffleDown.glsl");
            }

            if ((info.HelperFunctionsMask & HelperFunctionsMask.ShuffleUp) != 0)
            {
                AppendHelperFunction(context, "Ryujinx.Graphics.Shader/CodeGen/Glsl/HelperFunctions/ShuffleUp.glsl");
            }

            if ((info.HelperFunctionsMask & HelperFunctionsMask.ShuffleXor) != 0)
            {
                AppendHelperFunction(context, "Ryujinx.Graphics.Shader/CodeGen/Glsl/HelperFunctions/ShuffleXor.glsl");
            }

            if ((info.HelperFunctionsMask & HelperFunctionsMask.SwizzleAdd) != 0)
            {
                AppendHelperFunction(context, "Ryujinx.Graphics.Shader/CodeGen/Glsl/HelperFunctions/SwizzleAdd.glsl");
            }
        }

        public static void DeclareLocals(CodeGenContext context, StructuredFunction function)
        {
            foreach (AstOperand decl in function.Locals)
            {
                string name = context.OperandManager.DeclareLocal(decl);

                context.AppendLine(GetVarTypeName(decl.VarType) + " " + name + ";");
            }
        }

        public static string GetVarTypeName(VariableType type)
        {
            switch (type)
            {
                case VariableType.Bool: return "bool";
                case VariableType.F32: return "precise float";
                case VariableType.F64: return "double";
                case VariableType.None: return "void";
                case VariableType.S32: return "int";
                case VariableType.U32: return "uint";
            }

            throw new ArgumentException($"Invalid variable type \"{type}\".");
        }

        private static void DeclareUniforms(CodeGenContext context, BufferDescriptor[] descriptors)
        {
            string ubSize = "[" + NumberFormatter.FormatInt(Constants.ConstantBufferSize / 16) + "]";

            if (context.Config.UsedFeatures.HasFlag(FeatureFlags.CbIndexing))
            {
                string ubName = OperandManager.GetShaderStagePrefix(context.Config.Stage);

                ubName += "_" + DefaultNames.UniformNamePrefix;

                string blockName = $"{ubName}_{DefaultNames.BlockSuffix}";

                context.AppendLine($"layout (binding = {context.Config.FirstConstantBufferBinding}, std140) uniform {blockName}");
                context.EnterScope();
                context.AppendLine("vec4 " + DefaultNames.DataName + ubSize + ";");
                context.LeaveScope($" {ubName}[{NumberFormatter.FormatInt(descriptors.Max(x => x.Slot) + 1)}];");
            }
            else
            {
                foreach (var descriptor in descriptors)
                {
                    string ubName = OperandManager.GetShaderStagePrefix(context.Config.Stage);

                    ubName += "_" + DefaultNames.UniformNamePrefix + descriptor.Slot;

                    context.AppendLine($"layout (binding = {descriptor.Binding}, std140) uniform {ubName}");
                    context.EnterScope();
                    context.AppendLine("vec4 " + OperandManager.GetUbName(context.Config.Stage, descriptor.Slot, false) + ubSize + ";");
                    context.LeaveScope(";");
                }
            }
        }

        private static void DeclareStorages(CodeGenContext context, BufferDescriptor[] descriptors)
        {
            string sbName = OperandManager.GetShaderStagePrefix(context.Config.Stage);

            sbName += "_" + DefaultNames.StorageNamePrefix;

            string blockName = $"{sbName}_{DefaultNames.BlockSuffix}";

            string layout = context.Config.Options.TargetApi == TargetApi.Vulkan ? ", set = 1" : string.Empty;

            context.AppendLine($"layout (binding = {context.Config.FirstStorageBufferBinding}{layout}, std430) buffer {blockName}");
            context.EnterScope();
            context.AppendLine("uint " + DefaultNames.DataName + "[];");
            context.LeaveScope($" {sbName}[{NumberFormatter.FormatInt(descriptors.Max(x => x.Slot) + 1)}];");
        }

        private static void DeclareSamplers(CodeGenContext context, TextureDescriptor[] descriptors)
        {
            int arraySize = 0;
            foreach (var descriptor in descriptors)
            {
                if (descriptor.Type.HasFlag(SamplerType.Indexed))
                {
                    if (arraySize == 0)
                    {
                        arraySize = ShaderConfig.SamplerArraySize;
                    }
                    else if (--arraySize != 0)
                    {
                        continue;
                    }
                }

                string indexExpr = NumberFormatter.FormatInt(arraySize);

                string samplerName = OperandManager.GetSamplerName(
                    context.Config.Stage,
                    descriptor.CbufSlot,
                    descriptor.HandleIndex,
                    descriptor.Type.HasFlag(SamplerType.Indexed),
                    indexExpr);

                string samplerTypeName = descriptor.Type.ToGlslSamplerType();

                string layout = string.Empty;

                if (context.Config.Options.TargetApi == TargetApi.Vulkan)
                {
                    bool isBuffer = (descriptor.Type & SamplerType.Mask) == SamplerType.TextureBuffer;
                    int setIndex = isBuffer ? 4 : 2;

                    layout = $", set = {setIndex}";
                }

                context.AppendLine($"layout (binding = {descriptor.Binding}{layout}) uniform {samplerTypeName} {samplerName};");
            }
        }

        private static void DeclareImages(CodeGenContext context, TextureDescriptor[] descriptors)
        {
            int arraySize = 0;
            foreach (var descriptor in descriptors)
            {
                if (descriptor.Type.HasFlag(SamplerType.Indexed))
                {
                    if (arraySize == 0)
                    {
                        arraySize = ShaderConfig.SamplerArraySize;
                    }
                    else if (--arraySize != 0)
                    {
                        continue;
                    }
                }

                string indexExpr = NumberFormatter.FormatInt(arraySize);

                string imageName = OperandManager.GetImageName(
                    context.Config.Stage,
                    descriptor.CbufSlot,
                    descriptor.HandleIndex,
                    descriptor.Format,
                    descriptor.Type.HasFlag(SamplerType.Indexed),
                    indexExpr);

                string imageTypeName = descriptor.Type.ToGlslImageType(descriptor.Format.GetComponentType());

                string layout = descriptor.Format.ToGlslFormat();

                if (!string.IsNullOrEmpty(layout))
                {
                    layout = ", " + layout;
                }

                if (context.Config.Options.TargetApi == TargetApi.Vulkan)
                {
                    bool isBuffer = (descriptor.Type & SamplerType.Mask) == SamplerType.TextureBuffer;
                    int setIndex = isBuffer ? 5 : 3;

                    layout = $", set = {setIndex}{layout}";
                }

                context.AppendLine($"layout (binding = {descriptor.Binding}{layout}) uniform {imageTypeName} {imageName};");
            }
        }

        private static void DeclareInputAttributes(CodeGenContext context, StructuredProgramInfo info)
        {
            if (context.Config.GpPassthrough)
            {
                for (int attr = 0; attr < MaxAttributes; attr++)
                {
                    DeclareInputAttribute(context, info, attr);
                }

                foreach (int attr in info.IAttributes.OrderBy(x => x).Where(x => x >= MaxAttributes))
                {
                    DeclareInputAttribute(context, info, attr);
                }
            }
            else
            {
                foreach (int attr in info.IAttributes.OrderBy(x => x))
                {
                    DeclareInputAttribute(context, info, attr);
                }
            }
        }

        private static void DeclareInputAttribute(CodeGenContext context, StructuredProgramInfo info, int attr)
        {
            string suffix = context.Config.Stage == ShaderStage.Geometry ? "[]" : string.Empty;
            string iq = string.Empty;

            if (context.Config.Stage == ShaderStage.Fragment)
            {
                iq = context.Config.ImapTypes[attr].GetFirstUsedType() switch
                {
                    PixelImap.Constant => "flat ",
                    PixelImap.ScreenLinear => "noperspective ",
                    _ => string.Empty
                };
            }

            string pass = context.Config.GpPassthrough && !info.OAttributes.Contains(attr) ? "passthrough, " : string.Empty;

            string name = $"{DefaultNames.IAttributePrefix}{attr}";

            if (context.Config.Options.Flags.HasFlag(TranslationFlags.Feedback))
            {
                string type;

                if (context.Config.Stage == ShaderStage.Vertex)
                {
                    type = context.Config.GpuAccessor.QueryAttributeType(attr).GetScalarType();
                }
                else
                {
                    type = AttributeType.Float.GetScalarType();
                }

                for (int c = 0; c < 4; c++)
                {
                    char swzMask = "xyzw"[c];

                    context.AppendLine($"layout ({pass}location = {attr}, component = {c}) {iq}in {type} {name}_{swzMask}{suffix};");
                }
            }
            else
            {
                string type;

                if (context.Config.Stage == ShaderStage.Vertex)
                {
                    type = context.Config.GpuAccessor.QueryAttributeType(attr).GetVec4Type();
                }
                else
                {
                    type = AttributeType.Float.GetVec4Type();
                }

                context.AppendLine($"layout ({pass}location = {attr}) {iq}in {type} {name}{suffix};");
            }
        }

        private static void DeclareOutputAttributes(CodeGenContext context, StructuredProgramInfo info)
        {
            if (context.Config.Stage == ShaderStage.Fragment || context.Config.GpPassthrough)
            {
                DeclareUsedOutputAttributes(context, info);
            }
            else
            {
                DeclareAllOutputAttributes(context, info);
            }
        }

        private static void DeclareUsedOutputAttributes(CodeGenContext context, StructuredProgramInfo info)
        {
            foreach (int attr in info.OAttributes.OrderBy(x => x))
            {
                DeclareOutputAttribute(context, attr);
            }
        }

        private static void DeclareAllOutputAttributes(CodeGenContext context, StructuredProgramInfo info)
        {
            for (int attr = 0; attr < MaxAttributes; attr++)
            {
                DeclareOutputAttribute(context, attr);
            }

            foreach (int attr in info.OAttributes.OrderBy(x => x).Where(x => x >= MaxAttributes))
            {
                DeclareOutputAttribute(context, attr);
            }
        }

        private static void DeclareOutputAttribute(CodeGenContext context, int attr)
        {
            string name = $"{DefaultNames.OAttributePrefix}{attr}";

            if (context.Config.Options.Flags.HasFlag(TranslationFlags.Feedback))
            {
                for (int c = 0; c < 4; c++)
                {
                    char swzMask = "xyzw"[c];

                    string xfb = string.Empty;

                    var tfOutput = context.GetTransformFeedbackOutput(attr, c);
                    if (tfOutput.Valid)
                    {
                        xfb = $", xfb_buffer = {tfOutput.Buffer}, xfb_offset = {tfOutput.Offset}, xfb_stride = {tfOutput.Stride}";
                    }

                    context.AppendLine($"layout (location = {attr}, component = {c}{xfb}) out float {name}_{swzMask};");
                }
            }
            else
            {
                context.AppendLine($"layout (location = {attr}) out vec4 {name};");
            }
        }

        private static void DeclareSupportUniformBlock(CodeGenContext context, bool isFragment, int scaleElements)
        {
            if (!isFragment && scaleElements == 0)
            {
                return;
            }

            context.AppendLine($"layout (binding = 0, std140) uniform {DefaultNames.SupportBlockName}");
            context.EnterScope();

            if (isFragment)
            {
                context.AppendLine($"uint {DefaultNames.SupportBlockAlphaTestName};");
                context.AppendLine($"bool {DefaultNames.SupportBlockIsBgraName}[{SupportBuffer.FragmentIsBgraCount}];");
            }
            else
            {
                context.AppendLine($"uint s_reserved[{SupportBuffer.ComputeRenderScaleOffset / SupportBuffer.FieldSize}];");
            }

            if (scaleElements != 0)
            {
                context.AppendLine($"float {DefaultNames.SupportBlockRenderScaleName}[{scaleElements}];");
            }

            context.LeaveScope(";");
            context.AppendLine();
        }

        private static void AppendHelperFunction(CodeGenContext context, string filename)
        {
            string code = EmbeddedResources.ReadAllText(filename);

            code = code.Replace("\t", CodeGenContext.Tab);
            code = code.Replace("$SHARED_MEM$", DefaultNames.SharedMemoryName);
            code = code.Replace("$STORAGE_MEM$", OperandManager.GetShaderStagePrefix(context.Config.Stage) + "_" + DefaultNames.StorageNamePrefix);

            context.AppendLine(code);
            context.AppendLine();
        }
    }
}