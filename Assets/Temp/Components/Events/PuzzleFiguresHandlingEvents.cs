using SevenBoldPencil.EasyEvents;

namespace Temp.Components.Events
{
    /// <summary>
    /// Создается при взятии фигуры или ее отпускании (если ей нужно вернуться на место).
    /// Удаляется в системе изменения масштаба фигуры.
    /// </summary>
    public struct ChangeFigureScaleComponent : IEventSingleton
    {
        public int Entity;
        public bool Increase;
    }

    /// <summary>
    /// Создается после спавна фигур в SpawnFiguresSystem, изменяется в AssignPowerUpToFigureSystem, удаляется
    /// в AddPowerUpOnFigureSystem.
    /// </summary>
    public struct AddPowerUpEvent : IEventSingleton
    {
        public bool[] Data;
    }
}