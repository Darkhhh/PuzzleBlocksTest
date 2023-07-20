using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Source.Code.Common.Effects
{
    public class ScorePopUpTextScript : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private TextMeshPro textMeshPro;

        
        [Space] [Header("Effect Settings")]
        [SerializeField]
        [Tooltip("Animation time before text starts disappearing")] 
        private float timeToDisappear;

        
        [SerializeField] private float disappearSpeed;

        private Color _textColor;
        private float _disappearTimer;
        private Vector3 _moveVector;

        public void Init()
        {
            _textColor = Color.white;
            textMeshPro.color = _textColor;
            _disappearTimer = timeToDisappear;
            _moveVector = new Vector3(.7f, 1) * 60f;
        }

        public void PlayEffect(int score, Action callback = null)
        {
            textMeshPro.SetText($"+{score}");
            StartCoroutine(Effect(callback));
        }

        private IEnumerator Effect(Action callback = null)
        {
            transform.position += _moveVector * Time.deltaTime;
            _moveVector -= _moveVector * (8f * Time.deltaTime);

            while (_disappearTimer > 0)
            {
                if (_disappearTimer > timeToDisappear * .5f) {
                    var increaseScaleAmount = 1f;
                    transform.localScale += Vector3.one * (increaseScaleAmount * Time.deltaTime);
                } else {
                    var decreaseScaleAmount = 1f;
                    transform.localScale -= Vector3.one * (decreaseScaleAmount * Time.deltaTime);
                }
                _disappearTimer -= Time.deltaTime;
                yield return null;
            }

            while (_textColor.a > 0)
            {
                _textColor.a -= disappearSpeed * Time.deltaTime;
                textMeshPro.color = _textColor;
                yield return null;
            }
            
            callback?.Invoke();
        }
    }
}