using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HSR.NPRShader.Editor.Tools
{
    public abstract class BaseMaterialSetter
    {
        public bool TrySet(MaterialInfo srcMatInfo, Material dstMaterial)
        {
            if (!SupportedShaderMap.TryGetValue(srcMatInfo.Shader, out string shader))
            {
                return false;
            }

            MaterialUtility.SetShaderAndResetProperties(dstMaterial, shader);
            SetProperties(srcMatInfo, dstMaterial);
            return true;
        }

        public bool TryCreate(MaterialInfo matInfo, out Material material)
        {
            if (!SupportedShaderMap.TryGetValue(matInfo.Shader, out string shader))
            {
                material = null;
                return false;
            }

            material = new Material(Shader.Find(shader));
            SetProperties(matInfo, material);
            return true;
        }

        private void SetProperties(MaterialInfo matInfo, Material material)
        {
            foreach (var (key, value) in ApplyTextures(EntriesToDict(matInfo.Textures)))
            {
                if (value.IsNull)
                {
                    material.SetTexture(key, null);
                }
                else if (!string.IsNullOrWhiteSpace(value.Name))
                {
                    string[] textures = AssetDatabase.FindAssets($"t:{nameof(Texture)} {value.Name}");

                    if (textures.Length > 0)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(textures[0]);
                        Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(path);
                        material.SetTexture(key, texture);
                    }
                    else
                    {
                        Debug.LogWarning($"Texture {value.Name} was not found.");
                    }
                }

                material.SetTextureScale(key, value.Scale);
                material.SetTextureOffset(key, value.Offset);
            }

            foreach (var (key, value) in ApplyInts(EntriesToDict(matInfo.Ints)))
            {
                material.SetInt(key, value);
            }

            foreach (var (key, value) in ApplyFloats(EntriesToDict(matInfo.Floats)))
            {
                material.SetFloat(key, value);
            }

            foreach (var (key, value) in ApplyColors(EntriesToDict(matInfo.Colors)))
            {
                material.SetColor(key, value);
            }
        }

        private static Dictionary<string, T> EntriesToDict<T>(List<MaterialInfo.Entry<T>> entries)
        {
            return entries.ToDictionary(e => e.Key, e => e.Value);
        }

        public static Dictionary<string, BaseMaterialSetter> AllShaderMap
        {
            get
            {
                Dictionary<string, BaseMaterialSetter> map = new();

                foreach (var setterType in TypeCache.GetTypesDerivedFrom<BaseMaterialSetter>())
                {
                    var setter = (BaseMaterialSetter)Activator.CreateInstance(setterType);

                    foreach (string key in setter.SupportedShaderMap.Keys)
                    {
                        if (!map.TryGetValue(key, out BaseMaterialSetter oldSetter) || setter.Order < oldSetter.Order)
                        {
                            map[key] = setter;
                        }
                    }
                }

                return map;
            }
        }

        /// <summary>
        /// 越小优先级越高
        /// </summary>
        protected virtual int Order => 9999;

        protected abstract IReadOnlyDictionary<string, string> SupportedShaderMap { get; }

        protected virtual IEnumerable<(string, MaterialInfo.TextureInfo)> ApplyTextures(IReadOnlyDictionary<string, MaterialInfo.TextureInfo> textures)
        {
            yield break;
        }

        protected virtual IEnumerable<(string, int)> ApplyInts(IReadOnlyDictionary<string, int> ints)
        {
            yield break;
        }

        protected virtual IEnumerable<(string, float)> ApplyFloats(IReadOnlyDictionary<string, float> floats)
        {
            yield break;
        }

        protected virtual IEnumerable<(string, Color)> ApplyColors(IReadOnlyDictionary<string, Color> colors)
        {
            yield break;
        }
    }
}
