using Temp;
using UnityEngine;

namespace PuzzleCore
{
    public class Cell : MonoBehaviour
    {
        #region Contants

        public const float Size = 1f;

        #endregion
    
    
        #region Colors

        private static readonly Color AvailableColor = Color.white;
        private static readonly Color HighlightedColor = Color.green;
        private static readonly Color UnAvailableColor = Color.gray;

        #endregion


        #region Properties

        public Vector3 Position => gameObject.transform.position;

        public Vector3 RelevantPosition => _position;

        public Vector3 ParentPosition => _parent.transform.position;
    
        public bool Available { get; set; } = true;

        public bool ShouldBeCleared { get; set; } = false;

        public bool OrderedForPlacement { get; set; } = false;

        public bool AnchorBlockPlacement { get; set; } = false;

        public PowerUp PowerUp { get; set; } = PowerUp.None;

        #endregion


        #region Private Values

        private SpriteRenderer _renderer;
        private Transform _parent;
        private Vector3 _position;

        #endregion
    

    

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _parent = transform.parent;
            _position = _parent.transform.position + gameObject.transform.position;
        }

        private void SetColor(Color color)
        {
            _renderer.color = color;
        }

        public void SetAvailable()
        {
            ShouldBeCleared = false;
            Available = true;
            AnchorBlockPlacement = false;
            OrderedForPlacement = false;
            PowerUp = PowerUp.None;
            SetColor(AvailableColor);
        }

        public void SetUnavailable()
        {
            Available = false;
            ShouldBeCleared = false;
            AnchorBlockPlacement = false;
            OrderedForPlacement = false;
            SetColor(UnAvailableColor);
        }

        public void SetHighlighted()
        {
            OrderedForPlacement = true;
            SetColor(HighlightedColor);
        }
    }
}
