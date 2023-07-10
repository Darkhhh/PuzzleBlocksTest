using Source.Code.Views;
using Source.Code.Views.Cell;
using UnityEngine;

namespace Source.Code.Components
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

    
    /// <summary>
    /// Обозначает клетку, на которой расположено усиление.
    /// </summary>
    public struct CellPowerUpComponent
    {
        public PowerUpType Type;

        public PowerUpView View;
    }
    
    
    /// <summary>
    /// Обозначает клетку, с которой нужно удалить усиление без активации.
    /// </summary>
    public struct RemovePowerUpComponent { }
}