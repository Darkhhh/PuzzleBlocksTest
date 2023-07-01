namespace Source.Code.Views.Cell
{
    public class OccupiedCellState : CellState
    {
        public override void OnEnterState(CellView context)
        {
            context.PuzzleBlock.SetActive(true);
        }

        public override void OnExitState(CellView context)
        {
            context.PuzzleBlock.SetActive(false);
        }

        public override bool CanBeChangedOn(CellStateEnum state)
        {
            return state is CellStateEnum.Default or CellStateEnum.Destroyable or CellStateEnum.Targeted;
        }
    }
}