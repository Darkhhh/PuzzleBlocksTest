using Temp.Views;
using Temp.Views.Cell;
using UnityEngine;

namespace Temp.Components
{
    /// <summary>
    /// Основной компонент клетки доски. Никогда не удаляется. Создается при иницализации сцены.
    /// </summary>
    public struct CellComponent
    {
        public CellView View;
        
        public Vector3 Position => View.transform.position;
    }


    /// <summary>
    /// Обозначает клетку, на которой расположен якорный блок фигуры. Если такой клетки нет, то значит, что фигура
    /// либо находится вне доски, либо не может быть на ней расположена.
    /// </summary>
    public struct AnchoredBlockCellComponent { }

    public struct ShouldBeClearedCellComponent { }

    public struct CellPowerUpComponent
    {
        public PowerUpType Type;

        public PowerUpView View;
    }
    
    public struct DefaultCellStateComponent { }
    
    public struct SuggestedCellStateComponent { }

    public struct HighlightedCellStateComponent
    {
        public CellStateEnum PreviousState;
    }
    
    public struct OccupiedCellStateComponent { }

    public struct DestroyableCellStateComponent
    {
        public CellStateEnum PreviousState;
    }
    
    public struct TargetedCellStateComponent { }

    public struct ChangeCellStateComponent
    {
        public CellStateEnum State;
    }
}