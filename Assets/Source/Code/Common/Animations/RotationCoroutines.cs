using System;
using System.Collections;
using UnityEngine;

namespace Source.Code.Common.Animations
{
    public static class RotationCoroutines
    {
        public static IEnumerator Rotate(Transform obj, Quaternion valueMagnitude, float rotationTime, Action onEndRotation = null)
        {
            float timeElapsed = 0;
            var rotation = obj.rotation;
            var targetRotation = rotation * valueMagnitude;
            while (timeElapsed < rotationTime)
            {
                obj.rotation = Quaternion.Slerp(rotation, targetRotation, timeElapsed / rotationTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            obj.rotation = targetRotation;
            
            onEndRotation?.Invoke();
        }
        
        public static IEnumerator FullRotateY(Transform obj, float duration, Action onEndRotation = null)
        {
            var startRotation = obj.eulerAngles.y;
            var endRotation = startRotation + 360.0f;
            var t = 0.0f;
            while ( t  < duration )
            {
                t += Time.deltaTime;
                var yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
                var eulerAngles = obj.eulerAngles;
                eulerAngles = new Vector3(eulerAngles.x, yRotation, eulerAngles.z);
                obj.eulerAngles = eulerAngles;
                yield return null;
            }
            
            onEndRotation?.Invoke();
        }
    }
}