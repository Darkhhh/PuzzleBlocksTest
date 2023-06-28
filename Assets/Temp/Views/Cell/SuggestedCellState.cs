using UnityEngine;

namespace Temp.Views.Cell
{
    public class SuggestedCellState : CellState
    {
        private static readonly Color SuggestionColor = Color.cyan;
        
        public override void OnEnterState(CellView context)
        {
            context.Renderer.color = SuggestionColor;
        }

        public override void OnExitState(CellView context)
        {
            
        }

        public override bool CanBeChangedOn(CellStateEnum state)
        {
            return state is CellStateEnum.Default or CellStateEnum.Destroyable or CellStateEnum.Occupied;
        }
    }
}