using SevenBoldPencil.EasyEvents;

namespace PuzzleCore.ECS.Components.Events
{
    public struct DecreaseAllFiguresScaleComponent : IEventSingleton { }

    
    public struct ChangeFigureScaleComponent : IEventSingleton
    {
        public int Entity;
        public bool Increase;
    }

    public struct AddPowerUpEvent : IEventSingleton
    {
        public bool[] Data;
    }
}