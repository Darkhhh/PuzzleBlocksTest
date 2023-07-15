using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.Components;
using Source.Code.Mono;
using Source.Code.SharedData;
using Source.Code.Views;

namespace Source.Code.AnimationSystems
{
    public class AnimateDestroyableArmorBlocksSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CellComponent, RemovePowerUpComponent, CellPowerUpComponent>> _powerUpCellsFilter = default;
        
        private DestroyingArmorBlocksHandler _handler;
        
        public void Init(IEcsSystems systems)
        {
            _handler = systems.GetShared<SystemsSharedData>().SceneData.destroyingArmorBlocksHandler;
            _handler.Init(8);
        }

        public void Run(IEcsSystems systems)
        {
            if (_powerUpCellsFilter.Value.GetEntitiesCount() == 0) return;

            foreach (var cellEntity in _powerUpCellsFilter.Value)
            {
                ref var powerUp = ref _powerUpCellsFilter.Pools.Inc3.Get(cellEntity);
                
                if (powerUp.Type != PowerUpType.ArmoredBlock) continue;
                
                ref var cell = ref _powerUpCellsFilter.Pools.Inc1.Get(cellEntity);
                
                _handler.ActivateEffect(cell.Position);
            }
        }
    }
}