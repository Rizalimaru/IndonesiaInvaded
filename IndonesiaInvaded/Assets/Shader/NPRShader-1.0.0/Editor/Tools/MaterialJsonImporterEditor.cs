using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace HSR.NPRShader.Editor.Tools
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MaterialJsonImporter))]
    internal class MaterialJsonImporterEditor : ScriptedImporterEditor
    {
        private static readonly Lazy<GUIStyle> s_WrapMiniLabelStyle = new(() =>
        {
            return new GUIStyle(EditorStyles.miniLabel) { wordWrap = true };
        });

        private SerializedProperty m_OverrideShaderName;

        public override void OnEnable()
        {
            base.OnEnable();

            m_OverrideShaderName = serializedObject.FindProperty("m_OverrideShaderName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            ShaderOverrideDropdown();

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("The results are for reference only. It is recommended that you further adjust the detailed properties yourself.", s_WrapMiniLabelStyle.Value);

                using (new EditorGUI.DisabledScope(hasUnsavedChanges))
                {
                    GenerateMaterialButton();
                    OverwriteMaterialButton();
                }
            }

            ApplyRevertGUI();
        }

        private void ShaderOverrideDropdown()
        {
            Rect rect = EditorGUILayout.GetControlRect(true);
            rect = EditorGUI.PrefixLabel(rect, EditorGUIUtility.TrTextContent("Shader Override"));

            if (EditorGUI.DropdownButton(rect, EditorGUIUtility.TrTextContent(m_OverrideShaderName.stringValue), FocusType.Passive))
            {
                GenericMenu menu = new();

                foreach (var shaderName in BaseMaterialSetter.AllShaderMap.Keys.OrderBy(x => x))
                {
                    bool isOn = shaderName == m_OverrideShaderName.stringValue;
                    menu.AddItem(new GUIContent(shaderName), isOn, n =>
                    {
                        m_OverrideShaderName.stringValue = (string)n;
                        serializedObject.ApplyModifiedProperties();
                    }, shaderName);
                }

                menu.DropDown(rect);
            }

            EditorGUILayout.HelpBox("If the Shader field below is missing, you can manually specify it here.", MessageType.Info);
        }

        private void GenerateMaterialButton()
        {
            if (!GUILayout.Button("Generate Material"))
            {
                return;
            }

            Dictionary<string, BaseMaterialSetter> shaderMap = BaseMaterialSetter.AllShaderMap;

            foreach (MaterialInfo matInfo in assetTargets.Select(x => x as MaterialInfo))
            {
                bool ok = false;

                if (shaderMap.TryGetValue(matInfo.Shader, out BaseMaterialSetter setter))
                {
                    if (setter.TryCreate(matInfo, out Material material))
                    {
                        string path = AssetDatabase.GetAssetPath(matInfo);
                        path = Path.ChangeExtension(path, ".mat");
                        path = AssetDatabase.GenerateUniqueAssetPath(path);

                        AssetDatabase.CreateAsset(material, path);
                        ok = true;
                    }
                }

                if (!ok)
                {
                    Debug.LogError($"Failed to generate material for {matInfo.name}.", matInfo);
                }
            }
        }

        private void OverwriteMaterialButton()
        {
            if (!GUILayout.Button("Overwrite Material"))
            {
                return;
            }

            string assetPath = AssetDatabase.GetAssetPath(assetTargets[0]);
            string matFilePath = EditorUtility.OpenFilePanelWithFilters("Select Material",
                Path.GetDirectoryName(assetPath), new[] { "Material files", "mat" });

            if (string.IsNullOrEmpty(matFilePath) || !matFilePath.StartsWith(Application.dataPath + "/"))
            {
                return;
            }

            matFilePath = "Assets" + matFilePath.Substring(Application.dataPath.Length);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(matFilePath);

            if (material == null)
            {
                Debug.LogError($"Invalid material file: {matFilePath}");
                return;
            }

            Dictionary<string, BaseMaterialSetter> shaderMap = BaseMaterialSetter.AllShaderMap;
            bool ok = false;

            foreach (MaterialInfo matInfo in assetTargets.Select(x => x as MaterialInfo))
            {
                if (shaderMap.TryGetValue(matInfo.Shader, out BaseMaterialSetter setter))
                {
                    if (setter.TrySet(matInfo, material))
                    {
                        ok = true;
                        break;
                    }
                }
            }

            if (!ok)
            {
                Debug.LogError($"Failed to overwrite material {material.name}.", material);
            }
        }
    }
}
