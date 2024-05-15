using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HSR.NPRShader.Editor.Tools
{
    public static class MaterialUtility
    {
        public static void SetShaderAndResetProperties(Material material, string shaderName)
        {
            Shader shader = Shader.Find(shaderName);
            material.shader = shader;

            SerializedObject matObj = new(material);

            try
            {
                CleanProperties(shader, matObj.FindProperty("m_SavedProperties.m_TexEnvs"));
                CleanProperties(shader, matObj.FindProperty("m_SavedProperties.m_Floats"));
                CleanProperties(shader, matObj.FindProperty("m_SavedProperties.m_Ints"));
                CleanProperties(shader, matObj.FindProperty("m_SavedProperties.m_Colors"));
            }
            finally
            {
                matObj.ApplyModifiedProperties();
            }
        }

        private static void CleanProperties(Shader shader, SerializedProperty propArray)
        {
            if (!propArray.isArray)
            {
                return;
            }

            for (int i = propArray.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty prop = propArray.GetArrayElementAtIndex(i);
                string propName = prop.FindPropertyRelative("first").stringValue;
                int propIndex = shader.FindPropertyIndex(propName);

                if (propIndex >= 0)
                {
                    SerializedProperty propValue = prop.FindPropertyRelative("second");

                    // 重置为默认值
                    switch (propValue.propertyType)
                    {
                        case SerializedPropertyType.Float:
                            propValue.floatValue = shader.GetPropertyDefaultFloatValue(propIndex);
                            break;

                        case SerializedPropertyType.Integer:
                            propValue.intValue = shader.GetPropertyDefaultIntValue(propIndex);
                            break;

                        case SerializedPropertyType.Color:
                            propValue.colorValue = shader.GetPropertyDefaultVectorValue(propIndex);
                            break;

                        // Texture
                        case SerializedPropertyType.Generic:
                        {
                            SerializedProperty texture = propValue.FindPropertyRelative("m_Texture");
                            SerializedProperty scale = propValue.FindPropertyRelative("m_Scale");
                            SerializedProperty offset = propValue.FindPropertyRelative("m_Offset");

                            texture.objectReferenceValue = null;
                            scale.vector2Value = Vector2.one;
                            offset.vector2Value = Vector2.zero;
                            break;
                        }

                        default:
                            throw new NotSupportedException($"Material property type {propValue.propertyType} is not supported.");
                    }
                }
                else
                {
                    // 删除多余的 property
                    propArray.DeleteArrayElementAtIndex(i);
                }
            }
        }

        [MenuItem("Assets/Import as StarRail Material Json")]
        private static void ImportAsHSRMaterialJson()
        {
            foreach (Object obj in Selection.objects)
            {
                if (!EditorUtility.IsPersistent(obj) || obj is not TextAsset)
                {
                    continue;
                }

                string path = AssetDatabase.GetAssetPath(obj);
                AssetDatabase.SetImporterOverride<MaterialJsonImporter>(path);
            }
        }

        [MenuItem("Assets/Import as StarRail Material Json", isValidateFunction: true)]
        private static bool ImportAsHSRMaterialJsonValidate()
        {
            return Selection.objects.Any(obj => EditorUtility.IsPersistent(obj) && obj is TextAsset);
        }
    }
}
