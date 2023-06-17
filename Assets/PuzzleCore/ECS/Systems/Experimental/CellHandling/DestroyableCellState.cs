using UnityEngine;

namespace PuzzleCore.ECS.Systems.Experimental.CellHandling
{
    public class DestroyableCellState : CellState
    {
        private static readonly Vector3 ScaleVector = new (0.1f, 0.1f, 0.1f);
        
        public override void OnEnterState(Cell context)
        {
            context.PuzzleBlock.transform.localScale += ScaleVector;
        }

        public override void OnExitState(Cell context)
        {
            context.PuzzleBlock.transform.localScale -= ScaleVector;
        }

        public override bool CanBeChangedOn(CellStateEnum state)
        {
            return state is CellStateEnum.Default or CellStateEnum.Occupied or CellStateEnum.Suggested;
        }
    }
}