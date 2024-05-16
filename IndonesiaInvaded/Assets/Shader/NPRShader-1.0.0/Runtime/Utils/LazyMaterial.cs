using UnityEngine;
using UnityEngine.Rendering;

namespace HSR.NPRShader.Utils
{
    public class LazyMaterial
    {
        private readonly string m_ShaderName;
        private Material m_Material;

        public LazyMaterial(string shaderName)
        {
            m_ShaderName = shaderName;
        }

        public Material Value
        {
            get
            {
                if (m_Material == null)
                {
                    var shader = Shader.Find(m_ShaderName);
                    m_Material = CoreUtils.CreateEngineMaterial(shader);
                }

                return m_Material;
            }
        }

        public void DestroyCache()
        {
            CoreUtils.Destroy(m_Material);
            m_Material = null;
        }
    }
}
