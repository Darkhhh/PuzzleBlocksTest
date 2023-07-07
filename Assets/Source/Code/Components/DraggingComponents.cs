using Source.Code.Views;
using UnityEngine;

namespace Source.Code.Components
{
    /// <summary>
    /// Обозначает объект, который может быть перетаскиваемым по сцене.
    /// </summary>
    public struct DraggableObjectComponent
    {
        public IDraggableObject View;
    }

    
    /// <summary>
    /// Обозначает объект, который при перетаскивании над доской, влияет на состояние клеток доски.
    /// </summary>
    public struct DraggableOverGridComponent
    {
        public IGridPlaceableObject PlaceableObject;

        public bool MustBeFullOnGrid;

        public bool CheckOnCellAvailability;
    }

    
    /// <summary>
    /// Добавляется при взятии перетаскиваемого объекта (DraggableObjectComponent).
    /// </summary>
    public struct DraggingObjectComponent
    {
        public Vector3 Offset;
        public Vector3 InitialPosition;
    }

    
    /// <summary>
    /// Добавляется при взятии перетаскиваемого над доской объекта (DraggableOverGridComponent).
    /// </summary>
    public struct DraggingOverGridComponent
    {
        public IGridPlaceableObject PlaceableObject;

        public Vector3 CurrentPosition;

        public bool MustBeFullOnGrid;

        public bool CheckOnCellAvailability;
    }

    
    /// <summary>
    /// Добавляется при отпускании перетаскиваемого объекта (DraggingObjectComponent).
    /// </summary>
    public struct ReleasedObjectComponent
    {
        public Vector3 InitialPosition;
    }
    
    
    /// <summary>
    /// Добавляется на объект, который нельзя перетаскивать.
    /// </summary>
    public struct DoNotTakeObject { }
}