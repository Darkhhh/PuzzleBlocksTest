using SevenBoldPencil.EasyEvents;

namespace Source.Code.Components.Events
{
    /// <summary>
    /// Создается в ReleaseManualPowerUpSystem если есть Target клетки.
    /// Уничтожается в ClearTargetedCellsSystem после обработки.
    /// </summary>
    public struct ClearTargetedCellsEvent : IEventSingleton { }
}