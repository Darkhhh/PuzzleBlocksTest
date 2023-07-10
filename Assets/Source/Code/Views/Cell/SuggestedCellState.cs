using UnityEngine;

namespace Source.Code.Views.Cell
{
    public class SuggestedCellState : CellState
    {
        private static readonly Color SuggestionColor = Color.cyan;
        private static readonly Color DefaultColor = Color.white;
        
        public override void OnEnterState(CellView context)
        {
            context.Renderer.color = SuggestionColor;
        }

        public override void OnExitState(CellView context)
        {
            context.Renderer.color = DefaultColor;
        }

        public override bool CanBeChangedOn(CellStateEnum state)
        {
            return state is CellStateEnum.Default or CellStateEnum.Destroyable or CellStateEnum.Occupied;
        }
    }
}