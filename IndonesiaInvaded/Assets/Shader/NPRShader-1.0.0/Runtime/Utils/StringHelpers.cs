using System.Runtime.CompilerServices;
using UnityEngine;

namespace HSR.NPRShader.Utils
{
    public static class StringHelpers
    {
        public static int ShaderPropertyIDFromMemberName([CallerMemberName] string name = null)
        {
            return Shader.PropertyToID(name);
        }

        public static string MemberName([CallerMemberName] string name = null)
        {
            return name;
        }
    }
}
