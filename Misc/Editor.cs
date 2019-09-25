using System;
using Kit.Utils;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kit.Unity
{
    public static partial class Misc
    {
#if UNITY_EDITOR
        public static void HelpBox(MessageType type, params object[] args) =>
            EditorGUILayout.HelpBox(args.StringJoin("\n").Trim(), type);

        public static void HelpBox(params object[] args) =>
            HelpBox(MessageType.None, args);


        // utility method
        static GUIStyle horizontalLine;
        public static void HorizontalLine(Color color, float margin = 2)
        {
            if (horizontalLine == null)
            {
                horizontalLine = new GUIStyle();
                horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
                horizontalLine.margin = new RectOffset(0, 0, 4, 4);
                horizontalLine.fixedHeight = 1;
            }

            var c = GUI.color;
            GUI.color = color;
            GUILayout.Space(margin);
            GUILayout.Box(GUIContent.none, horizontalLine);
            GUILayout.Space(margin);
            GUI.color = c;
        }
        public static void HorizontalLine(float margin = 2) =>
            HorizontalLine(new Color(0, 0, 0, .15f), margin);


        public static void WithinVertical(Action action)
        {
            GUIStyle style = new GUIStyle(EditorStyles.helpBox);
            style.padding = new RectOffset(10, 10, 10, 10);
            EditorGUILayout.BeginVertical(style);

            action();

            EditorGUILayout.EndVertical();
        }

        public static void IfEditorChanged(Action action, Action then)
        {
            EditorGUI.BeginChangeCheck();

            action();

            if (EditorGUI.EndChangeCheck())
                then();
        }
#endif
    }
}
