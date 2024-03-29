﻿using System;
using UnityEngine;

namespace Kit
{
    public static partial class Misc
    {
        public static void WithGuiEnabled(bool enabled, Action action)
        {
            bool before = GUI.enabled;
            GUI.enabled = enabled;
            action();
            GUI.enabled = before;
        }

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
