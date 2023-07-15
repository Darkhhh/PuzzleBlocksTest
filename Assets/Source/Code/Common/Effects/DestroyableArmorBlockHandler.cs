using System;
using System.Collections.Generic;
using Source.Code.Common.Animations;
using Source.Code.Common.Utils;
using UnityEngine;

namespace Source.Code.Common.Effects
{
    public class DestroyableArmorBlockHandler : MonoBehaviour
    {
        [SerializeField] private float radius;
        [SerializeField] private float movingSpeed;
        
        private readonly Dictionary<Transform, Vector3> _children = new();
        private int _movingParts;
        
        
        public DestroyableArmorBlockHandler Init()
        {
            foreach (Transform child in transform) _children.Add(child, child.position);
            return this;
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }
        
        public void StartDestroying(Action callback = null)
        {
            _movingParts = _children.Count;
            
            foreach (var item in _children)
            {
                var randomPosition = RandomGenerator.RandomPointOnCircle(transform.position, radius);

                StartCoroutine(MovingCoroutines.MoveTowards(
                    item.Key, 
                    randomPosition, 
                    movingSpeed, 
                    () =>
                    {
                        _movingParts--;
                        if (_movingParts <= 0) callback?.Invoke();
                    })
                );
            }
        }

        public void ResetPositions()
        {
            foreach (var item in _children)
            {
                item.Key.position = item.Value;
            }
        }
    }
}
