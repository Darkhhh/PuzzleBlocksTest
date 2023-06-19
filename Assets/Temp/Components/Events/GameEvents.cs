using SevenBoldPencil.EasyEvents;

namespace Temp.Components.Events
{
    /// <summary>
    /// Создается после спавна новых фигур, или установки одной из существующих на доску
    /// Уничтожается в FindPlaceForFiguresSystem сразу после обработки
    /// </summary>
    public struct FindPlaceForFiguresEvent : IEventSingleton { }
    
    
    /// <summary>
    /// Создается после спавна фигур, уничтожается автоматически в том же кадре в CleanUpEntryEcsSystems
    /// </summary>
    public struct FiguresWereSpawnedEvent : IEventSingleton { }
}