using SevenBoldPencil.EasyEvents;

namespace PuzzleCore.ECS.Components.Events
{
    public struct HighlightGridEvent : IEventSingleton { }
    
    public struct DeHighlightGridEvent : IEventSingleton { }
    
    public struct RoughClearEvent : IEventSingleton { }
}