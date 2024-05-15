using HSR.NPRShader.Utils;
using UnityEditor;
using UnityEngine;

namespace HSR.NPRShader.Editor.AssetProcessors
{
    internal class AvatarModelPostprocessor : AssetPostprocessor
    {
        private void OnPreprocessModel()
        {
            AssetProcessorConfig config = AssetProcessorGlobalSettings.instance.AvatarModelProcessConfig;
            context.DependsOnCustomDependency(AssetProcessorGlobalSettings.AvatarModelDependencyName);

            if (config.IsEnableAndAssetPathMatch(assetPath))
            {
                config.ApplyPreset(context, assetImporter);
            }
        }

        private void OnPostprocessModel(GameObject go)
        {
            AssetProcessorConfig config = AssetProcessorGlobalSettings.instance.AvatarModelProcessConfig;

            if (config.IsEnableAndAssetPathMatch(assetPath))
            {
                NormalUtility.SmoothAndStore(go, config.SmoothNormalStoreMode, false);
                go.AddComponent<StarRailCharacterRenderingController>();
            }
        }

        public override uint GetVersion() => 31u;
    }
}
