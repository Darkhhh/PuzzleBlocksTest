namespace PuzzleCore.ECS.Systems.Experimental.CellHandling
{
    public abstract class CellState
    {
        public abstract void OnEnterState(Cell context);
        
        public abstract void OnExitState(Cell context);

        public abstract bool CanBeChangedOn(CellStateEnum state);
    }
}