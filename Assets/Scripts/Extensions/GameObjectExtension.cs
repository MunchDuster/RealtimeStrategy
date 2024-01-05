using UnityEngine;

namespace Munchy.Extensions
{
    public static class GameObjectExtension
    {
        public static void SetLayerRecursively(this GameObject obj, LayerMask layer)
        {
            obj.layer = layer.GetLayer();

            foreach (Transform child in obj.transform)
                child.gameObject.SetLayerRecursively(layer);
        }
    }
}