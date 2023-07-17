using UnityEngine;

namespace Source.Code.Common.Effects
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PuzzleFigureBlockScript : MonoBehaviour
    {
        [SerializeField] private Material defaultMaterial;

        [SerializeField] private Material grayScaleMaterial;

        private SpriteRenderer _spriteRenderer;

        private Material _defaultMaterial, _grayScaleMaterial;

        
        public PuzzleFigureBlockScript Init()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultMaterial = new Material(defaultMaterial);
            _grayScaleMaterial = new Material(grayScaleMaterial);

            return this;
        }

        public void SetToDefault()
        {
            _spriteRenderer.material = _defaultMaterial;
        }

        public void SetToGrayScale()
        {
            _spriteRenderer.material = _grayScaleMaterial;
        }
    }
}
