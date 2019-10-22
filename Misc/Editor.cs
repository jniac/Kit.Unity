using System;
using Kit;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kit
{
    public static partial class Misc
    {
#if UNITY_EDITOR

        static bool CoolFontPlzDone = false;
        public static void CoolFontPlz()
        {
            if (CoolFontPlzDone)
                return;

            try
            {
                Font font = Resources.Load<Font>("Fonts/Roboto_Mono/RobotoMono-Regular");
                EditorStyles.popup.font = font;
                EditorStyles.label.font = font;
                EditorStyles.helpBox.font = font;

                CoolFontPlzDone = true;
            }
#pragma warning disable CS0168 // La variable est déclarée mais jamais utilisée
#pragma warning disable RECS0022 // Clause catch qui intercepte System.Exception et dont le corps est vide
            catch (Exception e)
#pragma warning restore RECS0022 // Clause catch qui intercepte System.Exception et dont le corps est vide
#pragma warning restore CS0168 // La variable est déclarée mais jamais utilisée
            {
                // Unity hasn't initialized his own object,
                // fail silently, next time will be the right time

                //Debug.LogWarning("oups, CoolFontPlzDone");
                //Debug.LogException(e);
            }
        }

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

        static RectOffset boxPadding; 
        static RectOffset BoxPadding => boxPadding ?? (boxPadding = new RectOffset(10, 10, 10, 10));
        public static void InsideBox(Action action)
        {
            GUIStyle style = new GUIStyle(EditorStyles.helpBox) { padding = BoxPadding };
            EditorGUILayout.BeginVertical(style);

            action();

            EditorGUILayout.EndVertical();
        }

        public static bool WideToggle(string label, bool value, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel, options);
            value = EditorGUILayout.Toggle(value, GUILayout.Width(16));
            EditorGUILayout.EndHorizontal();

            return value;
        }
#endif
    }
}
