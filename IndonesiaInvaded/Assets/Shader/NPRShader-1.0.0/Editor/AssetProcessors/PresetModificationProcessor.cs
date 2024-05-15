using UnityEditor;

namespace HSR.NPRShader.Editor.AssetProcessors
{
    internal class PresetModificationProcessor : AssetModificationProcessor
    {
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            UpdateAssetProcessorGlobalSettings(sourcePath, destinationPath);
            return AssetMoveResult.DidNotMove;
        }

        private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            UpdateAssetProcessorGlobalSettings(assetPath, null);
            return AssetDeleteResult.DidNotDelete;
        }

        private static void UpdateAssetProcessorGlobalSettings(string assetSourcePath, string assetDestinationPath)
        {
            var settings = AssetProcessorGlobalSettings.instance;
            bool changed = false;

            foreach (AssetProcessorConfig config in settings.AllConfigs)
            {
                if (config.OverridePresetAssetPath == assetSourcePath)
                {
                    config.OverridePresetAssetPath = assetDestinationPath;
                    changed = true;
                }
            }

            if (changed)
            {
                // https://docs.unity3d.com/ScriptReference/AssetModificationProcessor.OnWillMoveAsset.html
                // You should not call any Unity AssetDatabase API from within this callback,
                // preferably restrict yourself to the usage of file operations or VCS APIs.
                EditorApplication.delayCall += () => settings.Save();
            }
        }
    }
}
