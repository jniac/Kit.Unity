using System;
using UnityEngine;
using System.Linq;
using Kit;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kit
{
    public class MaterialInspector : MonoBehaviour
    {


#if UNITY_EDITOR
        [CustomEditor(typeof(MaterialInspector))]
        public class MyEditor : Editor
        {
            MaterialInspector Target => target as MaterialInspector;

            public override void OnInspectorGUI()
            {
                var materials = Target.GetComponent<MeshRenderer>().sharedMaterials;

                Misc.HelpBox(
                    $"materials({materials.Length})",
                    materials.Select((m, i) => $"{i}: Render Queue: {m.shader.renderQueue}").StringJoin("\n"),
                    null);
            }
        }
#endif
    }
}
