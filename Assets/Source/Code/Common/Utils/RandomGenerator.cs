using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Code.Common.Utils
{
    public static class RandomGenerator
    {
        public static Vector2 RandomPointOnCircle(Vector2 center, float radius)
        {
            var theta = Random.Range(0f, 1f) * 2 * Math.PI;
            var pointX = center.x + radius * Math.Cos(theta);
            var pointY = center.y + radius * Math.Sin(theta);
            return new Vector2((float) pointX, (float) pointY);
        }
    }
}