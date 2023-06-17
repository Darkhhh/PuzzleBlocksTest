using UnityEngine;

namespace PuzzleCore.ECS.Systems.Experimental.CellHandling
{
    public class DefaultCellState : CellState
    {
        private static readonly Color DefaultColor = Color.white;
        
        public override void OnEnterState(Cell context)
        {
            context.Renderer.color = DefaultColor;
        }

        public override void OnExitState(Cell context)
        {
            
        }

        public override bool CanBeChangedOn(CellStateEnum state)
        {
            return state is not CellStateEnum.Targeted;
        }

        public void OnRoughEnterState(Cell context)
        {
            context.Renderer.color = DefaultColor;
            context.PuzzleBlock.SetActive(false);
            context.TargetBlock.SetActive(false);
        }
    }
}