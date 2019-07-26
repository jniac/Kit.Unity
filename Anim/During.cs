using System;
namespace Kit.Unity
{
    public partial class Anim
    {
        public static Anim During(float duration, 
            Action<Anim> callback, 
            float delay = 0,
            object key = null, 
            bool autoKillSimilarKey = true, 
            bool autoKillNullifiedKey = true)
        {
            if (autoKillSimilarKey && key != null)
                Kill(key);

            return new Anim(key, callback, duration, delay, autoKillNullifiedKey);
        }
    }
}
