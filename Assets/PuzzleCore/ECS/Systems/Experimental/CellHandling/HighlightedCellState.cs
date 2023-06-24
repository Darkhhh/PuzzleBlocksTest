using UnityEngine;

namespace PuzzleCore.ECS.Systems.Experimental.CellHandling
{
    public class HighlightedCellState : CellState
    {
        private static readonly Color HighlightedColor = Color.green;
        private static readonly Color DefaultColor = Color.white;
        
        public override void OnEnterState(Cell context)
        {
            context.Renderer.color = HighlightedColor;
        }

        public override void OnExitState(Cell context)
        {
            context.Renderer.color = DefaultColor;
        }

        public override bool CanBeChangedOn(CellStateEnum state)
        {
            return state is CellStateEnum.Default or CellStateEnum.Destroyable or CellStateEnum.Occupied;
        }
    }
}