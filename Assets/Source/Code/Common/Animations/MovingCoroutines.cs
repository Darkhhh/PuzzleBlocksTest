using System;
using System.Collections;
using UnityEngine;

namespace Source.Code.Common.Animations
{
    public static class MovingCoroutines
    {
        public static IEnumerator MoveTo(this Transform t, Vector3 targetPosition, float speed, Action callback = null)
        {
            while (Vector3.Distance(t.position, targetPosition) > 0.1f)
            {
                t.position = Vector3.MoveTowards(t.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            callback?.Invoke();
        }
        
        public static IEnumerator MoveTowards(Transform t, Vector3 targetPosition, float speed, Action callback = null)
        {
            while (Vector3.Distance(t.position, targetPosition) > 0.1f)
            {
                t.position = Vector3.MoveTowards(t.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            t.position = targetPosition;
            callback?.Invoke();
        }
        
        public static IEnumerator MoveTowardsOverTime(Transform t, Vector3 targetPosition, float time, Action callback = null)
        {
            var elapsedTime = 0f;
            var initialPosition = t.position;
            while (elapsedTime < time)
            {
                t.position = Vector3.Lerp(initialPosition, targetPosition, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            t.position = targetPosition;
            callback?.Invoke();
        }
    }
}