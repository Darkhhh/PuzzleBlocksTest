namespace Temp.Views.Cell
{
    public abstract class CellState
    {
        public abstract void OnEnterState(CellView context);
        
        public abstract void OnExitState(CellView context);

        public abstract bool CanBeChangedOn(CellStateEnum state);
    }
}