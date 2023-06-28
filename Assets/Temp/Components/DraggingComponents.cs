using Temp.Views;
using UnityEngine;

namespace Temp.Components
{
    public struct DraggableObjectComponent
    {
        public IDraggableObject View;
    }

    public struct DraggableOverGridComponent
    {
        public IGridPlaceableObject PlaceableObject;

        public bool MustBeFullOnGrid;

        public bool CheckOnCellAvailability;
    }

    public struct DraggingObjectComponent
    {
        public Vector3 Offset;
        public Vector3 InitialPosition;
    }

    public struct DraggingOverGridComponent
    {
        public IGridPlaceableObject PlaceableObject;

        public Vector3 CurrentPosition;

        public bool MustBeFullOnGrid;

        public bool CheckOnCellAvailability;
    }

    public struct ReleasedObjectComponent
    {
        public Vector3 InitialPosition;
    }
}