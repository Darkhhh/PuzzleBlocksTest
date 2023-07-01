using Source.Code.Views.Cell;

namespace Source.Code.Components
{
    public struct ChangeCellStateComponent { public CellStateEnum State; }
    
    
    public struct DefaultCellStateComponent { }

    
    public struct SuggestedCellStateComponent { }

    
    public struct HighlightedCellStateComponent { public CellStateEnum PreviousState; }
    
    
    public struct OccupiedCellStateComponent { }

    
    public struct DestroyableCellStateComponent { public CellStateEnum PreviousState; }
    
    
    public struct TargetedCellStateComponent { }
}