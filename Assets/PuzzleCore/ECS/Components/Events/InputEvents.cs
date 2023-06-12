using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace PuzzleCore.ECS.Components.Events
{
    public struct LeftMouseDownEvent : IEventSingleton
    {
        public Vector3 Position;
    }
    public struct RightMouseDownEvent : IEventSingleton
    {
        public Vector3 Position;
    }

    public struct CurrentMousePositionEvent : IEventSingleton
    {
        public Vector3 Position;
    }
}