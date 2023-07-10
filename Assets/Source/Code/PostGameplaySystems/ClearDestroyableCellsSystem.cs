using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Common.Utils;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.Mono;
using Source.Code.SharedData;
using Source.Code.Views.Cell;

namespace Source.Code.PostGameplaySystems
{
    /// <summary>
    /// Очищает Destroyable клетки и усиления с них, должна отрабатывать после активации всех усилений
    /// </summary>
    public class ClearDestroyableCellsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ShouldBeRemovedFigureComponent>> _removingFigureFilter = default;
        private readonly EcsFilterInject<Inc<CellComponent, DestroyableCellStateComponent>>_destroyableCellsFilter = default;

        private readonly EcsPoolInject<CellPowerUpComponent> _cellPowerUpsPool = default;
        private readonly EcsPoolInject<RemovePowerUpComponent> _clearCellPowerUpPool = default;

        private EventsBus _events;

        public void Init(IEcsSystems systems) => _events = systems.GetShared<SystemsSharedData>().EventsBus;
        
        
        public void Run(IEcsSystems systems)
        {
            if (_removingFigureFilter.Value.GetEntitiesCount() == 0) return;
            
            var earnedPoints = _destroyableCellsFilter.Value.GetEntitiesCount();

            if (earnedPoints > 0)
            {
                if (!_events.HasEventSingleton<IntermediateResultEvent>())
                {
                    ref var data = ref _events.NewEventSingleton<IntermediateResultEvent>();
                    data.DestroyedCells = earnedPoints;
                }
                else
                {
                    ref var data = ref _events.GetEventBodySingleton<IntermediateResultEvent>();
                    data.DestroyedCells = earnedPoints;
                }
            }

            foreach (var cellEntity in _destroyableCellsFilter.Value)
            {
                CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(cellEntity), CellStateEnum.Default);
                
                if (_cellPowerUpsPool.Value.Has(cellEntity))
                {
                    _clearCellPowerUpPool.Value.Add(cellEntity);
                }
            }
        }

        
    }
}