using SevenBoldPencil.EasyEvents;

namespace Source.Code.Components.Events
{
    /// <summary>
    /// Создается при нажатии на кнопку обмена. Уничтожается сразу после обработки в SwapFiguresAndPowerUpsSystem.
    /// </summary>
    public struct SwapFiguresAndPowerUpsEvent : IEventSingleton { }
    
    
    /// <summary>
    /// Создается при нажатии кнопки перезапуска. Уничтожается сразу после обработки.
    /// </summary>
    public struct RestartGameEvent : IEventSingleton { }
}