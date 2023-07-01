using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.Mono;
using Source.Code.SharedData;
using Source.Code.Utils;
using Source.Code.Views.Cell;

namespace Source.Code.PostGameplaySystems
{
    public class ClearTargetedCellsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TargetedCellStateComponent, CellComponent>> _targetedCellsFilter = default;

        private readonly EcsPoolInject<CellPowerUpComponent> _cellsPowerUpsPool = default;

        private readonly PowerUpsHandler _handler;
        private EventsBus _events;
        
        public ClearTargetedCellsSystem(PowerUpsHandler handler) => _handler = handler;
        
        
        public void Init(IEcsSystems systems) => _events = systems.GetShared<SystemsSharedData>().EventsBus;

        
        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<ClearTargetedCellsEvent>()) return;

            foreach (var entity in _targetedCellsFilter.Value)
            {
                CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(entity), CellStateEnum.Default);
                if (_cellsPowerUpsPool.Value.Has(entity))
                {
                    ref var cellPowerUp = ref _cellsPowerUpsPool.Value.Get(entity);
                
                    _handler.ReturnPowerUp(cellPowerUp.View);
                    _cellsPowerUpsPool.Value.Del(entity);
                }
            }
            
            _events.DestroyEventSingleton<ClearTargetedCellsEvent>();
        }
    }
}