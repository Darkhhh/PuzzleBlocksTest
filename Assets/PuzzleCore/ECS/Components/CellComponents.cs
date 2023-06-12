using PuzzleCore.ECS.Views;
using UnityEngine;

namespace PuzzleCore.ECS.Components
{
    public struct CellComponent
    {
        public CellView View;

        public bool Available;

        public Vector3 Position => View.transform.position;
    }

    public struct CellOrderedForPlacementComponent { }

    public struct AnchoredBlockCellComponent { }

    public struct ShouldBeClearedCellComponent { }

    public struct CellPowerUpComponent
    {
        public PowerUpType Type;

        public PowerUpView View;
    }
}