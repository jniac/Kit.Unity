using UnityEngine;

namespace Kit
{
    public static class RequireComponentExtensions
    {
        public static T RequireComponent<T>(this GameObject gameObject)
            where T : Component
        {
            T component = gameObject.GetComponent<T>();

            if (!component)
                component = gameObject.AddComponent<T>();

            return component;
        }

        public static bool RemoveComponent<T>(this GameObject gameObject)
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

        public static void ToggleComponent<T>(this GameObject gameObject, bool componentIsRequired)
            where T : Component
        {
            if (componentIsRequired)
            {
                gameObject.RequireComponent<T>();
            }
            else
            {
                gameObject.RemoveComponent<T>();
            }
        }

        public static void ToggleComponent<T>(this GameObject gameObject)
            where T : Component
            => gameObject.ToggleComponent<T>((bool)gameObject.GetComponent<T>());





        public static T RequireComponent<T>(this Transform transform)
            where T : Component =>
            transform.gameObject.RequireComponent<T>();
    }
}
