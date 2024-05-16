namespace HSR.NPRShader.Shadow
{
    public readonly struct PerObjectShadowCasterHandle
    {
        internal readonly int Index;
        internal readonly int Version;

        internal PerObjectShadowCasterHandle(int index, int version)
        {
            Index = index;
            Version = version;
        }
    }
}
