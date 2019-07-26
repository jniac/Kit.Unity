using System;
using UnityEngine;

namespace Kit.Unity
{
    public partial class Anim
    {
        static Ticker ticker;

        [ExecuteInEditMode]
        class Ticker : MonoBehaviour
        {
            public static void Init()
            {
                if (!ticker)
                {
                    ticker = FindObjectOfType<Ticker>();

                    if (!ticker)
                    {
                        GameObject gameObject = GameObject.Find("Main") 
                            ?? new GameObject($"{typeof(Anim).Name}-{typeof(Ticker).Name}");

                        ticker = gameObject.AddComponent<Ticker>();
                    }
                }
            }

            private void Update()
            {
                UpdateAll(Time.deltaTime);
            }
        }
    }
}
