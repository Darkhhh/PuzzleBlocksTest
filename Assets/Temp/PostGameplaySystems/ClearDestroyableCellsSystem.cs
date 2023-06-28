using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Temp.Components;
using Temp.Components.Events;
using Temp.Mono;
using Temp.SharedData;
using Temp.Utils;
using Temp.Views.Cell;

namespace Temp.PostGameplaySystems
{
    /// <summary>
    /// Очищает Destroyable клетки и усиления с них, должна отрабатывать после активации всех усилений
    /// </summary>
    public class ClearDestroyableCellsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ShouldBeRemovedFigureComponent>> _removingFigureFilter =
            default;

        private readonly
            EcsFilterInject<Inc<CellComponent, DestroyableCellStateComponent>>_destroyableCellsFilter = default;

        private readonly EcsPoolInject<CellPowerUpComponent> _cellsPowerUpsPool = default;

        private readonly PowerUpsHandler _handler;
        private EventsBus _events;
        
        public ClearDestroyableCellsSystem(PowerUpsHandler handler)
        {
            _handler = handler;
        }
        
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

                if (_cellsPowerUpsPool.Value.Has(cellEntity))
                {
                    ref var cellPowerUp = ref _cellsPowerUpsPool.Value.Get(cellEntity);
                
                    _handler.ReturnPowerUp(cellPowerUp.View);
                    _cellsPowerUpsPool.Value.Del(cellEntity);
                }
            }
        }

        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }
    }
}