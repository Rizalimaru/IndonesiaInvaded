using System;
using System.Collections.Generic;
using UnityEngine;

namespace HSR.NPRShader.Editor.Tools
{
    public class MaterialInfo : ScriptableObject
    {
        [Serializable]
        public class TextureInfo
        {
            public string Name;
            public long PathId;
            public bool IsNull;
            public Vector2 Scale;
            public Vector2 Offset;
        }

        [Serializable]
        public class Entry<T>
        {
            public string Key;
            public T Value;
        }

        public string Name;
        public string Shader;
        public List<Entry<TextureInfo>> Textures;
        public int TexturesSkipCount;
        public List<Entry<int>> Ints;
        public int IntsSkipCount;
        public List<Entry<float>> Floats;
        public int FloatsSkipCount;
        public List<Entry<Color>> Colors;
        public int ColorsSkipCount;
    }
}
