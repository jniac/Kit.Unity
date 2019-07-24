using UnityEngine;

namespace Kit.Unity
{
    public static partial class Misc
    {
        public static void Print(params object[] args)
        {
            Debug.Log(string.Join(" ", args));
        }
    }
}
