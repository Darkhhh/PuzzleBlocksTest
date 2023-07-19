using Source.Code.Views;
using UnityEngine;

namespace Source.Code.Components 
{
    /// <summary>
    /// Основной компонент фигуры.
    /// </summary>
    public struct PuzzleFigureComponent
    {
        public PuzzleFigureView View;
    }
    
    
    /// <summary>
    /// Наличие компонента обозначает, что на фигуре есть усиление.
    /// </summary>
    public struct FigurePowerUpComponent
    {
        public int BlockNumber;

        public PowerUpType Type;

        public PowerUpView View;
    }
    
    
    /// <summary>
    /// Обозначает количество доступных мест для фигуры, если место одно,
    /// AnchorCellEntity укажет где именно оно находится.
    /// </summary>
    public struct AvailablePlacesComponent
    {
        public int Amount;

        // TODO Change to PackedEntity
        public int AnchorCellEntity;
    }
    
    
    /// <summary>
    /// Добавляется на фигуру, которая должна быть убрана в пул.
    /// </summary>
    public struct ShouldBeRemovedFigureComponent { }
    
    
    /// <summary>
    /// Добавляется на фигуру, для которой нет ни одного места на доске.
    /// </summary>
    public struct CanNotBeTakenComponent { }
}