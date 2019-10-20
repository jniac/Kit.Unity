using System.Linq;
using UnityEngine;

namespace Kit.Unity
{
    public static partial class Misc
    {
        public static Material WithRenderQueue(Material material, int renderQueue)
        {
            return new Material(material) { renderQueue = renderQueue };
        }

        public static GameObject WithRenderQueue(GameObject gameObject, int renderQueue)
        {
            foreach(var mr in gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                mr.sharedMaterials = mr.sharedMaterials
                    .Select(m => WithRenderQueue(m, renderQueue))
                    .ToArray();
            }

            return gameObject;
        }



        public static Material WithColor(Material material, Color color)
        {
            return new Material(material) { color = color };
        }

        public static GameObject WithColor(GameObject gameObject, Color color)
        {
            foreach (var mr in gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                mr.sharedMaterials = mr.sharedMaterials
                    .Select(m => WithColor(m, color))
                    .ToArray();
            }

            return gameObject;
        }

    }
}
