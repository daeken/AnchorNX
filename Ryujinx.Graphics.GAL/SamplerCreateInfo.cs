namespace Ryujinx.Graphics.GAL
{
    public struct SamplerCreateInfo
    {
        public MinFilter MinFilter { get; }
        public MagFilter MagFilter { get; }

        public bool SeamlessCubemap { get; }

        public AddressMode AddressU { get; }
        public AddressMode AddressV { get; }
        public AddressMode AddressP { get; }

        public CompareMode CompareMode { get; }
        public CompareOp   CompareOp   { get; }

        public ColorF BorderColor { get; }

        public float MinLod        { get; }
        public float MaxLod        { get; }
        public float MipLodBias    { get; }
        public float MaxAnisotropy { get; }

        public SamplerCreateInfo(
            MinFilter   minFilter,
            MagFilter   magFilter,
            bool        seamlessCubemap,
            AddressMode addressU,
            AddressMode addressV,
            AddressMode addressP,
            CompareMode compareMode,
            CompareOp   compareOp,
            ColorF      borderColor,
            float       minLod,
            float       maxLod,
            float       mipLodBias,
            float       maxAnisotropy)
        {
            MinFilter       = minFilter;
            MagFilter       = magFilter;
            SeamlessCubemap = seamlessCubemap;
            AddressU        = addressU;
            AddressV        = addressV;
            AddressP        = addressP;
            CompareMode     = compareMode;
            CompareOp       = compareOp;
            BorderColor     = borderColor;
            MinLod          = minLod;
            MaxLod          = maxLod;
            MipLodBias      = mipLodBias;
            MaxAnisotropy   = maxAnisotropy;
        }
    }
}