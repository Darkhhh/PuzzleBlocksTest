using SevenBoldPencil.EasyEvents;

namespace Source.Code.Components.Events
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


    /// <summary>
    /// Создается в системах ActivateCoinSystem, ActivateMultipliersSystem или ClearDestroyableCellsSystem.
    /// Автоматически уничтожается в CountCoinsAndScoreSystem после подсчета промежуточного результата.
    /// </summary>
    public struct IntermediateResultEvent : IEventSingleton
    {
        public int CoinsAmount;

        public int Multiplier;

        public int DestroyedCells;
    }


    /// <summary>
    /// Создается в CountCoinsAndScoreSystem. Автоматически уничтожается в UpdateInGameUISystem.
    /// </summary>
    public struct UpdateInGameUIEvent : IEventSingleton
    {
        public int NewCoins;
        
        public int NewScore;
    }
}