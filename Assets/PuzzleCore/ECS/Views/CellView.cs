using UnityEngine;

namespace PuzzleCore.ECS.Views
{
    public class CellView : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private GameObject _puzzleBlock = null;
        private GameObject _target = null;
        #region Contants

        public const float Size = 1f;

        #endregion
    
        #region Colors

        private static readonly Color AvailableColor = Color.white;
        private static readonly Color HighlightedColor = Color.green;
        private static readonly Color UnAvailableColor = Color.gray;
        private static readonly Color SuggestionColor = Color.cyan;

        #endregion
    
        public Vector3 ParentPosition => _parent.transform.position;
        public bool Suggested { get; private set; }
        private Transform _parent;
    
        public void SetAvailable()
        {
            _target.SetActive(false);
            SetColor(AvailableColor);
        }

        public void SetHighlighted()
        {
            SetColor(HighlightedColor);
        }

        public void SetSuggestion()
        {
            Suggested = true;
            SetColor(SuggestionColor);
        }

        public void SetTarget()
        {
            _target.SetActive(true);
        }

        public void SetSimple()
        {
            _puzzleBlock.SetActive(false);
            Suggested = false;
            SetColor(AvailableColor);
        }

        public void SetUnAvailable()
        {
            _puzzleBlock.SetActive(true);
            Suggested = false;
            SetColor(UnAvailableColor);
        }
    
        private void SetColor(Color color)
        {
            _renderer.color = color;
        }

        public void Init()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _parent = transform.parent;
        }

        public void InjectPuzzleBlock(GameObject puzzleBlock)
        {
            var thisTransform = transform;
            
            puzzleBlock.transform.position = thisTransform.position;
            puzzleBlock.transform.parent = thisTransform;
            puzzleBlock.SetActive(false);
            
            _puzzleBlock = puzzleBlock;
        }

        public void InjectTarget(GameObject target)
        {
            var thisTransform = transform;
            
            target.transform.position = thisTransform.position;
            target.transform.parent = thisTransform;
            target.SetActive(false);
            _target = target;
        }
    }
}
