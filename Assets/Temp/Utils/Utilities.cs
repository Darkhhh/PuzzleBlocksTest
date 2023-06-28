using UnityEngine;
using static System.Math;

namespace Temp.Utils
{
    public static class Utilities
    {
        public static float CountDistance(Vector3 first, Vector3 second)
        {
            return (float) Sqrt(Pow(first.x - second.x, 2) + Pow(first.y - second.y, 2));
        }

        public static float CountDistanceNoSqrt(Vector3 first, Vector3 second)
        {
            return (float) (Pow(first.x - second.x, 2) + Pow(first.y - second.y, 2));
        }
    }
}