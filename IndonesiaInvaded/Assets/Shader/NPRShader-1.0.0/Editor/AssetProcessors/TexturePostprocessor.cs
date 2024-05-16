using UnityEditor;

namespace HSR.NPRShader.Editor.AssetProcessors
{
    internal class TexturePostprocessor : AssetPostprocessor
    {
        public override uint GetVersion() => 23u;

        private void OnPreprocessTexture()
        {
            var settings = AssetProcessorGlobalSettings.instance;
            context.DependsOnCustomDependency(AssetProcessorGlobalSettings.TexturesDependencyName);

            foreach (AssetProcessorConfig config in settings.TextureProcessConfigs)
            {
                if (config.IsEnableAndAssetPathMatch(assetPath))
                {
                    config.ApplyPreset(context, assetImporter);
                    break;
                }
            }
        }
    }
}
