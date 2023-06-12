using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Views;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.PowerUpHandling
{
    public class ActivateCoinPowerUpSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CellComponent, ShouldBeClearedCellComponent, CellPowerUpComponent>>
            _clearedCellsWithPowerUpsFilter = default;

        private readonly EcsPoolInject<CellPowerUpComponent> _cellPowerUpComponents = default;

        private readonly PowerUpsHandler _handler;
        
        public ActivateCoinPowerUpSystem(PowerUpsHandler handler)
        {
            _handler = handler;
        }
        
        public void Run(IEcsSystems systems)
        {
            if (_clearedCellsWithPowerUpsFilter.Value.GetEntitiesCount() == 0) return;

            var collectedCoins = 0;
            foreach (var cellEntity in _clearedCellsWithPowerUpsFilter.Value)
            {
                ref var cellPowerUp = ref _cellPowerUpComponents.Value.Get(cellEntity);
                
                if (cellPowerUp.Type != PowerUpType.Coin) continue;
                
                _handler.ReturnPowerUp(cellPowerUp.View);
                _cellPowerUpComponents.Value.Del(cellEntity);
                collectedCoins++;
                
            }
            
            Debug.Log($"Collected Coins: {collectedCoins}");
        }
    }
}