using UnityEngine;

namespace Source.Code.Views.Cell
{
    public class HighlightedCellState : CellState
    {
        public override void OnEnterState(CellView context)
        {
            context.PuzzleBlock.SetActive(true);
            
            var color = context.PuzzleBlockRenderer.color;
            var newColor = new Color(color.r, color.g, color.b, 0.75f);
            
            context.PuzzleBlockRenderer.color = newColor;
        }

        public override void OnExitState(CellView context)
        {
            var color = context.PuzzleBlockRenderer.color;
            var newColor = new Color(color.r, color.g, color.b, 1f);
            
            context.PuzzleBlockRenderer.color = newColor;
            context.PuzzleBlock.SetActive(false);
        }

        public override bool CanBeChangedOn(CellStateEnum state)
        {
            return state is CellStateEnum.Default or CellStateEnum.Destroyable or CellStateEnum.Occupied;
        }
    }
}