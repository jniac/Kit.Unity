using System;
using UnityEngine;

namespace Kit.Unity
{
    public static partial class Misc
    {
        public static Action WithGuiDisabled
        {
            set 
            {
                bool before = GUI.enabled;
                GUI.enabled = false;
                value();
                GUI.enabled = before;
            }
        }
    }
}
