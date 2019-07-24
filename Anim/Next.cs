using System;

namespace Kit.Unity
{
    public partial class Anim
    {
        public static void Next(Action callback, object key = null)
            => new Anim(key, anim => callback(), 0, 0, true);
    }
}
