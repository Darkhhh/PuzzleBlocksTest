using Source.Code.Views;
using UnityEngine;

namespace Source.Code.Common.Utils
{
    public static class Extensions
    {
        public static Vector3Int GetIntVector(this Vector3 t)
        {
            return new Vector3Int(Mathf.RoundToInt(t.x), Mathf.RoundToInt(t.y), Mathf.RoundToInt(t.z));
        }

        public static int Multiplier(this PowerUpType type)
        {
            return type switch
            {
                PowerUpType.MultiplierX2 => 2,
                PowerUpType.MultiplierX5 => 5,
                PowerUpType.MultiplierX10 => 10,
                _ => 1
            };
        }
    }
}