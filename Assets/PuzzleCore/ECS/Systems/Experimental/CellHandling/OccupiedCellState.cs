namespace PuzzleCore.ECS.Systems.Experimental.CellHandling
{
    public class OccupiedCellState : CellState
    {
        public override void OnEnterState(Cell context)
        {
            context.PuzzleBlock.SetActive(true);
        }

        public override void OnExitState(Cell context)
        {
            context.PuzzleBlock.SetActive(false);
        }

        public override bool CanBeChangedOn(CellStateEnum state)
        {
            return state is CellStateEnum.Default or CellStateEnum.Destroyable or CellStateEnum.Targeted;
        }
    }
}