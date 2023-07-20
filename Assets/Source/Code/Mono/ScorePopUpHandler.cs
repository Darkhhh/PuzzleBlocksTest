using System.Collections.Generic;
using Source.Code.Common.Effects;
using UnityEngine;

namespace Source.Code.Mono
{
    public class ScorePopUpHandler : MonoBehaviour
    {
        [SerializeField] private ScorePopUpTextScript prefab;

        private Queue<ScorePopUpTextScript> _effects;

        public void Init(int capacity = 4)
        {
            _effects = new Queue<ScorePopUpTextScript>(capacity);
            prefab.gameObject.SetActive(false);
            for (var i = 0; i < capacity; i++)
            {
                _effects.Enqueue(Instantiate(prefab, Vector3.zero,Quaternion.identity, transform));
            }
        }

        public void PlayEffect(Vector3 position, int score)
        {
            var effect = _effects.TryDequeue(out var e) ? 
                e : Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            effect.transform.position = position;
            effect.gameObject.SetActive(true);
            effect.Init();
            effect.PlayEffect(score, () =>
            {
                effect.gameObject.SetActive(false);
                _effects.Enqueue(effect);
            });
        }
    }
}