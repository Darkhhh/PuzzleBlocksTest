using SevenBoldPencil.EasyEvents;

namespace PuzzleCore.ECS.Components.Events
{
    public struct CheckOnEndGameComponent : IEventSingleton { }
    
    public struct RestartGameEvent : IEventSingleton { }
}