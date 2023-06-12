using SevenBoldPencil.EasyEvents;

namespace PuzzleCore.ECS.Components.Events
{
    public struct UpdateScoreEvent : IEventSingleton
    {
        public int NewScore;
    }
}