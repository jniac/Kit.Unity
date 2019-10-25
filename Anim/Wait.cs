using System;
using System.Runtime.CompilerServices;

namespace Kit
{
    public partial class Anim
    {
        // Shorthand based on During(),
        // await Anim.Wait(.5f)
        // instead of
        // await Anim.During(.5f, a => {}).Completion
        public static AwaitableCompletion Wait(float duration,
            Action<Anim> callback = null,
            float delay = 0,
            object key = null,
            bool autoKillSimilarKey = true,
            bool autoKillNullifiedKey = true,
            bool preRunDelayedAnim = true)
        {
            return During(duration, callback, delay, key, autoKillSimilarKey, autoKillNullifiedKey, preRunDelayedAnim).Completion;
        }
    }
}
