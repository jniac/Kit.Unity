using System;
using UnityEngine;
using System.Linq;
using Kit;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kit
{
    [ExecuteAlways]
    public class ChangeRenderQueue : MonoBehaviour
    {
        public bool changeRenderQueue;
        public int renderQueue;

        private void OnValidate()
        {
            if (changeRenderQueue)
                foreach (var m in GetComponent<MeshRenderer>().sharedMaterials)
                    m.renderQueue = renderQueue;
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(ChangeRenderQueue))]
        class MyEditor : Editor
        {
            ChangeRenderQueue Target => target as ChangeRenderQueue;

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                string to(Material m) => $"{m.name}-{m.renderQueue}";

                Misc.HelpBox(string.Join(", ", Target.GetComponent<MeshRenderer>()
                    .sharedMaterials.Select(to)));
            }
        }
#endif
    }
}
