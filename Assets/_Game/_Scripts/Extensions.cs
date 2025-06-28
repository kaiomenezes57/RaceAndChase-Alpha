using UnityEngine;

namespace Game
{
    public static class VectorExtensions
    {
        public static Vector3 WithOffset(this Vector3 source, float? x = null, float? y = null, float? z = null)
        {
            var offset = new Vector3(x ?? 0f, y ??  0f, z ?? 0f);
            return source + offset;
        }
    }
}
