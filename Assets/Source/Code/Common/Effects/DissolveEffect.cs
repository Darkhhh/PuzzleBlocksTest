using UnityEngine;

namespace Source.Code.Common.Effects
{
    public class DissolveEffect : MonoBehaviour
    {
        [SerializeField] private Material material;

        private float _dissolveAmount;
        private bool _isDissolving;
        private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");

        // Update is called once per frame
        void Update()
        {
            if (_isDissolving)
            {
                _dissolveAmount = Mathf.Clamp01(_dissolveAmount + Time.deltaTime);
                material.SetFloat(DissolveAmount, _dissolveAmount);
            }
            else
            {
                _dissolveAmount = Mathf.Clamp01(_dissolveAmount - Time.deltaTime);
                material.SetFloat(DissolveAmount, _dissolveAmount);
            }

            if (Input.GetKey(KeyCode.T))
            {
                _isDissolving = true;
            }
            if (Input.GetKey(KeyCode.Y))
            {
                _isDissolving = false;
            }
        }
    }
}
