using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;

namespace PuzzleCore.ECS.Systems.GridHighLightning
{
    /// <summary>
    /// При перетаскивании объекта, который помечает клетки в радиусе действия компонентом CellOrderedForPlacementComponent,
    /// подсвечивает эти клетки в зависимости от объекта:
    /// 1. Если фигура - то подсветка в виде блока;
    /// 2. Если ручное усиление - то подсветка в виде мишени, но только на занятых клетках.
    /// </summary>
    public class ReHighlightGridSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region ECS Filters

        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;
        
        private readonly EcsFilterInject<Inc<CellOrderedForPlacementComponent, CellComponent>> _orderedCellsFilter = default;

        private readonly EcsFilterInject<Inc<DraggingObjectComponent, ManualPowerUp>> _dragPowerUpFilter = default;

        #endregion

        #region ECS Pools

        private readonly EcsPoolInject<CellComponent> _cellComponents = default;

        #endregion

        private EventsBus _events;
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }
        
        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<HighlightGridEvent>()) return;
            foreach (var entity in _cellsFilter.Value)
            {
                ref var cell = ref _cellComponents.Value.Get(entity);
                if(cell.View.Suggested) continue;
                cell.View.SetAvailable();
            }
            
            foreach (var entity in _orderedCellsFilter.Value)
            {
                ref var cell = ref _cellComponents.Value.Get(entity);
                if (_dragPowerUpFilter.Value.GetEntitiesCount() > 0)
                {
                    if (!cell.Available) cell.View.SetTarget();
                }
                else cell.View.SetHighlighted();
            }
        }
    }
}
