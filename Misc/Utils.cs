using System;

namespace Kit.Unity
{
    public static partial class Misc
    {
        public static void IfChanged<T>(T oldValue, T newValue, params Action<T>[] callbacks)
        {
            if (!oldValue.Equals(newValue))
                foreach(var callback in callbacks)
                    callback(newValue);
        }
    }
}
