using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kit.Unity
{
    public partial class Anim
    {
        /*
         * ExecuteInEditMode is very buggy.
         * The class seems to work, but let's be careful.
         */
        [ExecuteInEditMode]
        public class Ticker : MonoBehaviour
        {
            public static Ticker Instance { get; private set; }

            public static void Init()
            {
                if (!Instance)
                {
                    Instance = FindObjectOfType<Ticker>();

                    if (!Instance)
                    {
                        GameObject gameObject = new GameObject($"{typeof(Anim).Name}-{typeof(Ticker).Name}");
                        gameObject.hideFlags = HideFlags.HideAndDontSave;
                        gameObject.AddComponent<Ticker>();
                    }
                }
            }

            static int count;
            public readonly int instanceId = count++;

            bool disabled;

            private void OnEnable()
            {
                //(instanceId, GetInstanceID(), hideFlags).Print("OnEnable");

                disabled = false;

                if (instanceId == 0)
                {
                    Instance = this;

                    if (!Application.isPlaying)
                        print($"well we are in the editor, Anim will not work very well {GetInstanceID()}");
                }
            }

            private void OnDisable()
            {
                //(instanceId, GetInstanceID(), hideFlags).Print("OnDisable");

                disabled = true;
            }

            private void OnDestroy()
            {
                if (instanceId == 0)
                {
                    // reset
                    Instance = null;
                    count = 0;

                    print("Anim Instance 0 destroyed");
                }
            }

            private void Update()
            {
                if (instanceId == 0)
                {
                    if (Application.isPlaying)
                        UpdateAll(Time.deltaTime);
                }
                else
                {
                    // security check, destroy any duplicate
                    DestroyImmediate(gameObject);
                }
            }

#if UNITY_EDITOR
            [MenuItem("Tools/Kit/Init Anim (debug)")]
            static void InitAnim()
            {
                Init();
            }
            [MenuItem("Tools/Kit/Reset Anim (debug")]
            static void ResetAnim()
            {
                if (Instance)
                    DestroyImmediate(Instance.gameObject);
            }
#endif
        }
    }
}
