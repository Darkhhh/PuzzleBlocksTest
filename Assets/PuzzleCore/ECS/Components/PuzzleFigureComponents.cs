using PuzzleCore.ECS.Views;
using UnityEngine;

namespace PuzzleCore.ECS.Components 
{
    struct PuzzleFigureComponent
    {
        public PuzzleFigureView View;

        public Vector3 AnchorBlockPosition => View.transform.position;

        public Vector3[] RelativeBlockPositions => View.BlocksRelativePositions;
    }
    
    public struct DraggingFigureComponent
    {
        public Vector3 Offset;

        public Vector3 InitialPosition;
    }

    struct ShouldBeRemovedFigureComponent { }

    struct FigurePowerUpComponent
    {
        public int BlockNumber;

        public PowerUpType Type;

        public PowerUpView View;
    }

    public struct AvailablePlacesComponent
    {
        public int Amount;

        public int AnchorCellEntity;
    }
    
}