namespace PuzzleCore.ECS.Systems.Experimental.CellHandling
{
    public class TargetedCellState : CellState
    {
        public override void OnEnterState(Cell context)
        {
            context.PuzzleBlock.SetActive(true);
            context.TargetBlock.SetActive(true);
        }

        public override void OnExitState(Cell context)
        {
            context.PuzzleBlock.SetActive(false);
            context.TargetBlock.SetActive(false);
        }

        public override bool CanBeChangedOn(CellStateEnum state)
        {
            return state is CellStateEnum.Occupied;
        }
    }
}