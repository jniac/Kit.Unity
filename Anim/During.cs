using System;
namespace Kit.Unity
{
    public partial class Anim
    {
        public static Anim During(float duration, 
            Action<Anim> callback, 
            float delay = 0,
            object key = null, 
            bool autoRemoveSimilarKey = true, 
            bool autoRemoveNullifiedKey = true)
        {
            if (autoRemoveSimilarKey)
            { } // TODO: remove existing anim

            return new Anim(key, callback, duration, delay, autoRemoveNullifiedKey);
        }
    }
}
