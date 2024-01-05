using UnityEngine;

namespace Munchy.Extensions
{
    public static class LayerMaskExtension
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            int maskOther = 1 << layer; // Convert the layer number to the mask with a 1 for the layer
            return (mask.value & maskOther) != 0;
        }

        public static int GetLayer(this LayerMask mask)
        {
            ulong maskOther = 1; // Convert the layer number to the mask with a 1 for the layer
            int words = sizeof(int) * 8;
            for (int i = 0; i < words; i++)
            {
                if (((ulong)mask.value & maskOther) != 0) return i;
                else maskOther <<= 1;
            }

            return -1;
        }
    }
}
