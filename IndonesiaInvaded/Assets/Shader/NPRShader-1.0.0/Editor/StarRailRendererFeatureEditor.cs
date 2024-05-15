using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEditor;

namespace HSR.NPRShader.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(StarRailRendererFeature))]
    public class StarRailRendererFeatureEditor : UnityEditor.Editor
    {
        public const string GitHubLink = "https://github.com/Tocorn";

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            foreach (UniversalRendererData rendererData in GetRendererDataList())
            {
                if (rendererData.renderingMode == RenderingMode.Deferred)
                {
                    EditorGUILayout.HelpBox("Deferred Rendering Path is not supported.", MessageType.Error);
                    EditorGUILayout.Space();
                }
                else if (rendererData.depthPrimingMode != DepthPrimingMode.Disabled)
                {
                    EditorGUILayout.HelpBox("Depth Priming is not supported.", MessageType.Error);
                    EditorGUILayout.Space();
                }
            }

            EditorGUILayout.LabelField("GitHub Repository", EditorStyles.boldLabel);

            if (EditorGUILayout.LinkButton(GitHubLink))
            {
                Application.OpenURL(GitHubLink);
            }
        }

        private List<UniversalRendererData> GetRendererDataList()
        {
            List<UniversalRendererData> rendererDataList = new();

            foreach (string path in targets.Select(AssetDatabase.GetAssetPath))
            {
                if (AssetDatabase.LoadMainAssetAtPath(path) is UniversalRendererData data)
                {
                    rendererDataList.Add(data);
                }
            }

            return rendererDataList;
        }
    }
}
