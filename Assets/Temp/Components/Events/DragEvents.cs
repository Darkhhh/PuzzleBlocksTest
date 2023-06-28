using SevenBoldPencil.EasyEvents;

namespace Temp.Components.Events
{
    /// <summary>
    /// Создается в DetectDraggableObjectSystem при взятии объекта, уничтожается автоматически в конце кадра
    /// </summary>
    public struct DraggableObjectWasTakenEvent : IEventSingleton { }
}