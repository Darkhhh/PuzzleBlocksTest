using UnityEngine;

namespace Source.Code.Views.Cell
{
    public class DefaultCellState : CellState
    {
        private static readonly Color DefaultColor = Color.white;
        
        public override void OnEnterState(CellView context)
        {
            context.Renderer.color = DefaultColor;
        }

        public override void OnExitState(CellView context)
        {
            
        }

        public override bool CanBeChangedOn(CellStateEnum state)
        {
            return state is not CellStateEnum.Targeted;
        }

        public void OnRoughEnterState(CellView context)
        {
            context.Renderer.color = DefaultColor;
            context.PuzzleBlock.SetActive(false);
            context.TargetBlock.SetActive(false);
        }
    }
}