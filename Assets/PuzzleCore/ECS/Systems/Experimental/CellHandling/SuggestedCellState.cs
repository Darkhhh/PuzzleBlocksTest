using UnityEngine;

namespace PuzzleCore.ECS.Systems.Experimental.CellHandling
{
    public class SuggestedCellState : CellState
    {
        private static readonly Color SuggestionColor = Color.cyan;
        
        public override void OnEnterState(Cell context)
        {
            context.Renderer.color = SuggestionColor;
        }

        public override void OnExitState(Cell context)
        {
            
        }

        public override bool CanBeChangedOn(CellStateEnum state)
        {
            return state is CellStateEnum.Default or CellStateEnum.Destroyable or CellStateEnum.Occupied;
        }
    }
}