namespace Source.Code.Views.Cell
{
    public class TargetedCellState : CellState
    {
        public override void OnEnterState(CellView context)
        {
            context.PuzzleBlock.SetActive(true);
            context.TargetBlock.SetActive(true);
        }

        public override void OnExitState(CellView context)
        {
            context.PuzzleBlock.SetActive(false);
            context.TargetBlock.SetActive(false);
        }

        public override bool CanBeChangedOn(CellStateEnum state)
        {
            return state is CellStateEnum.Occupied or CellStateEnum.Default;
        }
    }
}