using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Views;

namespace PuzzleCore.ECS.Systems.PowerUpHandling
{
    public class ActivateArmoredBlockPowerUpSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CellComponent, ShouldBeClearedCellComponent, CellPowerUpComponent>>
            _clearedCellsWithPowerUpsFilter = default;

        private readonly EcsPoolInject<CellPowerUpComponent> _cellPowerUpComponents = default;

        private readonly EcsPoolInject<ShouldBeClearedCellComponent> _shouldBeClearedCellComponents = default;

        private readonly PowerUpsHandler _handler;
        
        public ActivateArmoredBlockPowerUpSystem(PowerUpsHandler handler)
        {
            _handler = handler;
        }
        
        public void Run(IEcsSystems systems)
        {
            if (_clearedCellsWithPowerUpsFilter.Value.GetEntitiesCount() == 0) return;

            foreach (var cellEntity in _clearedCellsWithPowerUpsFilter.Value)
            {
                ref var cellPowerUp = ref _cellPowerUpComponents.Value.Get(cellEntity);
                
                if (cellPowerUp.Type != PowerUpType.ArmoredBlock) continue;
                
                _handler.ReturnPowerUp(cellPowerUp.View);
                
                _shouldBeClearedCellComponents.Value.Del(cellEntity);
                _cellPowerUpComponents.Value.Del(cellEntity);
                //ref var cell = ref _cellComponents.Value.Get(cellEntity);
                
            }
        }
    }
}