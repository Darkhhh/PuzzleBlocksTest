using UnityEngine;

namespace Source.Code.Views.Cell
{
    public class DestroyableCellState : CellState
    {
        private static readonly Vector3 ScaleVector = new (0.2f, 0.2f);
        
        public override void OnEnterState(CellView context)
        {
            context.PuzzleBlock.SetActive(true);
            context.PuzzleBlock.transform.localScale -= ScaleVector;
        }

        public override void OnExitState(CellView context)
        {
            context.PuzzleBlock.transform.localScale += ScaleVector;
            context.PuzzleBlock.SetActive(false);
        }

        public override bool CanBeChangedOn(CellStateEnum state)
        {
            return state is CellStateEnum.Default or CellStateEnum.Occupied or CellStateEnum.Suggested or CellStateEnum.Highlighted;
        }
    }
}