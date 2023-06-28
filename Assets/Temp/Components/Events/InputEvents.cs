using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace Temp.Components.Events
{
    /// <summary>
    /// Создается в HandleUserInputSystem, автоматически удаляется в конце кадра.
    /// </summary>
    public struct LeftMouseDownEvent : IEventSingleton
    {
        public Vector3 Position;
    }
    
    /// <summary>
    /// Создается в HandleUserInputSystem, автоматически удаляется в конце кадра.
    /// </summary>
    public struct RightMouseDownEvent : IEventSingleton
    {
        public Vector3 Position;
    }
}