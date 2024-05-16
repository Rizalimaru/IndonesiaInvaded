using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Rendering;

namespace HSR.NPRShader.Editor.Build
{
    internal class BuiltinShaderProvider : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            SerializedProperty shaderArray = GetAlwaysIncludedShaderArray();
            foreach (Shader shader in StarRailBuiltinShaders.Walk())
            {
                AddShaderIfNeeded(shaderArray, shader);
            }
            shaderArray.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            AssetDatabase.SaveAssets();
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            SerializedProperty shaderArray = GetAlwaysIncludedShaderArray();
            foreach (Shader shader in StarRailBuiltinShaders.Walk())
            {
                RemoveShaderIfNeeded(shaderArray, shader);
            }
            shaderArray.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            AssetDatabase.SaveAssets();
        }

        private static SerializedProperty GetAlwaysIncludedShaderArray()
        {
            SerializedObject settings = new(GraphicsSettings.GetGraphicsSettings());
            return settings.FindProperty("m_AlwaysIncludedShaders");
        }

        private static void AddShaderIfNeeded(SerializedProperty shaderArray, Shader shader)
        {
            for (int i = 0; i < shaderArray.arraySize; i++)
            {
                if (shaderArray.GetArrayElementAtIndex(i).objectReferenceValue == shader)
                {
                    return;
                }
            }

            int newIndex = shaderArray.arraySize;
            shaderArray.InsertArrayElementAtIndex(newIndex);
            shaderArray.GetArrayElementAtIndex(newIndex).objectReferenceValue = shader;
        }

        private static void RemoveShaderIfNeeded(SerializedProperty shaderArray, Shader shader)
        {
            for (int i = shaderArray.arraySize - 1; i >= 0; i--)
            {
                if (shaderArray.GetArrayElementAtIndex(i).objectReferenceValue == shader)
                {
                    shaderArray.DeleteArrayElementAtIndex(i);
                }
            }
        }
    }
}
