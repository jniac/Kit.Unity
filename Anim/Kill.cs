using System;
namespace Kit
{
    public partial class Anim
    {
        public static void Kill(object key)
        {
            foreach (var anim in dictionarySet.Get(key, returnEmptyArrayIfNull: true))
                anim.Destroy();
        }
    }
}
