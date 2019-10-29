using UnityEngine;

namespace Kit
{
    public static class RequireComponentExtensions
    {
        public static T Require<T>(this GameObject gameObject)
            where T : Component
        {
            T component = gameObject.GetComponent<T>();

            if (!component)
                component = gameObject.AddComponent<T>();

            return component;
        }

        public static bool Remove<T>(this GameObject gameObject)
            where T : Component
        {
            T component = gameObject.GetComponent<T>();

            if (component)
            {
                if (Application.isPlaying)
                    Object.Destroy(component);
                else
                    Object.DestroyImmediate(component);

                return true;
            }

            return false;
        }

        public static void Toggle<T>(this GameObject gameObject, bool componentIsRequired)
            where T : Component
        {
            if (componentIsRequired)
            {
                gameObject.Require<T>();
            }
            else
            {
                gameObject.Remove<T>();
            }
        }

        public static void Toggle<T>(this GameObject gameObject)
            where T : Component
            => gameObject.Toggle<T>((bool)gameObject.GetComponent<T>());





        public static T Require<T>(this Component component)
            where T : Component =>
            component.gameObject.Require<T>();
    }
}
