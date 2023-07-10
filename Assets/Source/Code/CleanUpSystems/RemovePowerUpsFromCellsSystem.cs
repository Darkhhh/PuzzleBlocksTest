using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.Components;
using Source.Code.Mono;

namespace Source.Code.CleanUpSystems
{
    public class RemovePowerUpsFromCellsSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CellComponent, RemovePowerUpComponent, CellPowerUpComponent>> _powerUpCellsFilter = default;
        
        private readonly PowerUpsHandler _handler;
        
        public RemovePowerUpsFromCellsSystem(PowerUpsHandler handler) => _handler = handler;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var cellEntity in _powerUpCellsFilter.Value)
            {
                ref var cellPowerUp = ref _powerUpCellsFilter.Pools.Inc3.Get(cellEntity);
                
                _handler.ReturnPowerUp(cellPowerUp.View);
                _powerUpCellsFilter.Pools.Inc2.Del(cellEntity);
                _powerUpCellsFilter.Pools.Inc3.Del(cellEntity);
            }
        }
    }
}