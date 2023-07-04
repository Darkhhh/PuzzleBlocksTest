using System;
using System.Collections;
using UnityEngine;

namespace Source.Code.Common.Effects
{
    public class DissolvingPuzzleBlockScript : MonoBehaviour
    {
        [SerializeField] private Material material;
        [SerializeField] private float movingSpeed;
        
        private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");
        private float _dissolveAmount;
        private SpriteRenderer _renderer;


        public void Init()
        {
            var newMaterial = new Material(material);
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.material = newMaterial;
        }
        
        
        public DissolvingPuzzleBlockScript Prepare(Vector3 position)
        {
            transform.position = position;
            _dissolveAmount = 0;
            _renderer.material.SetFloat(DissolveAmount, 0);
            return this;
        }

        public void MoveTowards(Vector3 targetPosition, Action callback = null)
        {
            StartCoroutine(Move(targetPosition, callback));
        }
        
        public void PlayEffect(Action callback = null)
        {
            StartCoroutine(Play(callback));
        }

        private IEnumerator Move(Vector3 targetPosition, Action callback = null)
        {
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, movingSpeed * Time.deltaTime);
                yield return null;
            }
            callback?.Invoke();
        }
        
        private IEnumerator Play(Action callback = null)
        {
            while (_dissolveAmount < 1)
            {
                _dissolveAmount = Mathf.Clamp01(_dissolveAmount + Time.deltaTime);
                _renderer.material.SetFloat(DissolveAmount, _dissolveAmount);
                yield return null;
            }
            callback?.Invoke();
        }
    }
}
