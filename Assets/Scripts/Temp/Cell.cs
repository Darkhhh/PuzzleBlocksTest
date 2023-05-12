using UnityEngine;

namespace Temp
{
    public class Cell
    {
        #region Static
        
        public const float Size = 1f;

        private static readonly Color AvailableColor = Color.white;
        private static readonly Color HighlightedColor = Color.green;
        private static readonly Color UnAvailableColor = Color.gray;

        #endregion
        
        
        #region Private Values

        private readonly GameObject _view;
        private readonly Transform _parent;
        private readonly SpriteRenderer _renderer;

        #endregion


        #region Properties

        public Vector3 Position => _view.transform.position;
        
        public Vector3 ParentPosition => _parent.transform.position;

        public CellStatus Status { get; set; } = CellStatus.Available;

        public AdditionalCellStatus AdditionalStatus { get; set; } = AdditionalCellStatus.None;

        #endregion


        #region Constructor

        public Cell(GameObject view, Transform parent)
        {
            _view = view;
            _parent = parent;
            _renderer = _view.GetComponent<SpriteRenderer>();
        }

        #endregion


        #region Private Methods

        private void SetColor(Color color)
        {
            _renderer.color = color;
        }

        #endregion


        #region Public Methods

        public void SetAvailable()
        {
            Status = CellStatus.Available;
            AdditionalStatus = AdditionalCellStatus.None;
            SetColor(AvailableColor);
        }

        public void SetUnavailable()
        {
            Status = CellStatus.UnAvailable;
            AdditionalStatus = AdditionalCellStatus.None;
            SetColor(UnAvailableColor);
        }

        public void SetHighlighted()
        {
            Status = CellStatus.OrderedForPlacement;
            AdditionalStatus = AdditionalCellStatus.None;
            SetColor(HighlightedColor);
        }

        #endregion
    }
    
    
    public enum CellStatus : byte
    {
        Available,
        OrderedForPlacement,
        UnAvailable,
    }

    
    public enum AdditionalCellStatus : byte
    {
        None,
        Anchored,
        ShouldBeCleared
    }
}