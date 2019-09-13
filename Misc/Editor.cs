using System;
using Kit.Utils;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kit.Unity
{
    public static partial class Misc
    {
#if UNITY_EDITOR
        public static void HelpBox(MessageType type, params object[] args) =>
            EditorGUILayout.HelpBox(args.StringJoin("\n"), type);

        public static void HelpBox(params object[] args) =>
            HelpBox(MessageType.None, args);
#endif
    }
}
