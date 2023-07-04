using System;
using UnityEngine;

namespace Source.Code.Views.Cell
{
    public enum CellStateEnum : byte
    {
        Default, Suggested, Highlighted, Occupied, Destroyable, Targeted
    }
    
    public class CellView : MonoBehaviour
    {
        #region States

        private readonly DefaultCellState _defaultCellState = new ();
        private readonly DestroyableCellState _destroyableCellState = new();
        private readonly HighlightedCellState _highlightedCellState = new();
        private readonly OccupiedCellState _occupiedCellState = new();
        private readonly SuggestedCellState _suggestedCellState = new();
        private readonly TargetedCellState _targetedCellState = new();

        #endregion


        #region Private Values

        private Vector3 _parentPosition;

        #endregion

        
        #region Properties

        private CellState CurrentState { get; set; }

        public GameObject PuzzleBlock { get; private set; }
        
        public SpriteRenderer PuzzleBlockRenderer { get; private set; }
        
        public SpriteRenderer Renderer { get; private set; }

        public GameObject TargetBlock { get; private set; }

        public Vector3 ParentPosition => _parentPosition;

        #endregion


        public void Init(GameObject puzzleBlock, GameObject targetBlock)
        {
            _parentPosition = transform.parent.position;
            
            PuzzleBlock = puzzleBlock;
            TargetBlock = targetBlock;
            Renderer = GetComponent<SpriteRenderer>();
            
            var thisTransform = transform;
            var position = thisTransform.position;
            
            PuzzleBlock.transform.position = position;
            PuzzleBlock.transform.parent = thisTransform;
            PuzzleBlock.SetActive(false);
            PuzzleBlockRenderer = PuzzleBlock.GetComponent<SpriteRenderer>();
            
            TargetBlock.transform.position = position;
            TargetBlock.transform.parent = thisTransform;
            TargetBlock.SetActive(false);

            CurrentState = _defaultCellState;
            CurrentState.OnEnterState(this);
        }


        public void ChangeState(CellStateEnum state)
        {
            if (!CurrentState.CanBeChangedOn(state)) return;
            
            CurrentState.OnExitState(this);
            CurrentState = GetState(state);
            CurrentState.OnEnterState(this);
        }

        private CellState GetState(CellStateEnum state)
        {
            return state switch
            {
                CellStateEnum.Default => _defaultCellState,
                CellStateEnum.Suggested => _suggestedCellState,
                CellStateEnum.Highlighted => _highlightedCellState,
                CellStateEnum.Occupied => _occupiedCellState,
                CellStateEnum.Destroyable => _destroyableCellState,
                CellStateEnum.Targeted => _targetedCellState,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}