using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HSR.NPRShader.Editor.AssetProcessors
{
    [FilePath("ProjectSettings/HSRAssetProcessors.asset", FilePathAttribute.Location.ProjectFolder)]
    public sealed class AssetProcessorGlobalSettings : ScriptableSingleton<AssetProcessorGlobalSettings>
    {
        public AssetProcessorConfig AvatarModelProcessConfig = new()
        {
            MatchMode = AssetPathMatchMode.NameGlob,
            PathPattern = @"Avatar_*_*.fbx | Art_*_*.fbx",
            DefaultPresetPath = @"Packages/star-rail-npr-shader/Presets/AvatarModel.preset",
        };

        public AssetProcessorConfig RampTextureProcessConfig = new()
        {
            MatchMode = AssetPathMatchMode.NameGlob,
            PathPattern = @"Avatar_*_Ramp*",
            DefaultPresetPath = @"Packages/star-rail-npr-shader/Presets/RampTexture.preset",
        };

        public AssetProcessorConfig LightMapProcessConfig = new()
        {
            MatchMode = AssetPathMatchMode.NameGlob,
            PathPattern = @"Avatar_*_LightMap*",
            DefaultPresetPath = @"Packages/star-rail-npr-shader/Presets/LightMap.preset",
        };

        public AssetProcessorConfig ColorTextureProcessConfig = new()
        {
            MatchMode = AssetPathMatchMode.NameGlob,
            PathPattern = @"Avatar_*_Color*",
            DefaultPresetPath = @"Packages/star-rail-npr-shader/Presets/ColorTexture.preset",
        };

        public AssetProcessorConfig StockingsRangeMapProcessConfig = new()
        {
            MatchMode = AssetPathMatchMode.NameGlob,
            PathPattern = @"Avatar_*_Stockings*",
            DefaultPresetPath = @"Packages/star-rail-npr-shader/Presets/StockingsRangeMap.preset",
        };

        public AssetProcessorConfig FaceMapProcessConfig = new()
        {
            MatchMode = AssetPathMatchMode.NameGlob,
            PathPattern = @"M_*_*_FaceMap* | W_*_*_FaceMap*",
            DefaultPresetPath = @"Packages/star-rail-npr-shader/Presets/FaceMap.preset",
        };

        public AssetProcessorConfig FaceExpressionMapProcessConfig = new()
        {
            MatchMode = AssetPathMatchMode.NameGlob,
            PathPattern = @"M_*_*_Face_ExpressionMap* | W_*_*_Face_ExpressionMap*",
            DefaultPresetPath = @"Packages/star-rail-npr-shader/Presets/FaceExpressionMap.preset",
        };

        public IEnumerable<AssetProcessorConfig> TextureProcessConfigs
        {
            get
            {
                yield return RampTextureProcessConfig;
                yield return LightMapProcessConfig;
                yield return ColorTextureProcessConfig;
                yield return StockingsRangeMapProcessConfig;
                yield return FaceMapProcessConfig;
                yield return FaceExpressionMapProcessConfig;
            }
        }

        public IEnumerable<AssetProcessorConfig> AllConfigs => TextureProcessConfigs.Prepend(AvatarModelProcessConfig);

        public void Save()
        {
            Save(true);
            RegisterToAssetDatabase();
        }

        private void RegisterToAssetDatabase()
        {
            Hash128 avatarModelHash = new();
            AvatarModelProcessConfig.AppendToHash(ref avatarModelHash, includeSmoothNormalStoreMode: true);
            AssetDatabase.RegisterCustomDependency(AvatarModelDependencyName, avatarModelHash);

            Hash128 texturesHash = new();
            foreach (AssetProcessorConfig textureProcessConfig in TextureProcessConfigs)
            {
                textureProcessConfig.AppendToHash(ref texturesHash);
            }
            AssetDatabase.RegisterCustomDependency(TexturesDependencyName, texturesHash);
        }

        public SerializedObject AsSerializedObject() => new(this);

        public const string PathInProjectSettings = "Project/StarRail NPR Shader/HSR Asset Processors";
        public const string AvatarModelDependencyName = "StarRail NPR Shader/HSR Asset Processors/AvatarModel";
        public const string TexturesDependencyName = "StarRail NPR Shader/HSR Asset Processors/Textures";

        public static void OpenInProjectSettings() => SettingsService.OpenProjectSettings(PathInProjectSettings);

        [InitializeOnLoadMethod]
        private static void InitAssetDatabaseCustomDependency()
        {
            instance.RegisterToAssetDatabase();
        }
    }
}
