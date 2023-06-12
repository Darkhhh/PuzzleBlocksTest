using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Common;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Views;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.PowerUpHandling
{
    public class ActivateMultiplierPowerUpSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CellComponent, ShouldBeClearedCellComponent, CellPowerUpComponent>>
            _clearedCellsWithPowerUpsFilter = default;

        private readonly EcsPoolInject<CellPowerUpComponent> _cellPowerUpComponents = default;

        private readonly PowerUpsHandler _handler;
        
        public ActivateMultiplierPowerUpSystem(PowerUpsHandler handler)
        {
            _handler = handler;
        }
        
        public void Run(IEcsSystems systems)
        {
            if (_clearedCellsWithPowerUpsFilter.Value.GetEntitiesCount() == 0) return;

            var multiplier = 1;
            foreach (var cellEntity in _clearedCellsWithPowerUpsFilter.Value)
            {
                ref var cellPowerUp = ref _cellPowerUpComponents.Value.Get(cellEntity);
                
                if (cellPowerUp.Type != PowerUpType.MultiplierX2 && 
                    cellPowerUp.Type != PowerUpType.MultiplierX5 &&
                    cellPowerUp.Type != PowerUpType.MultiplierX10) continue;

                multiplier *= cellPowerUp.Type.Multiplier();
                
                _handler.ReturnPowerUp(cellPowerUp.View);
                _cellPowerUpComponents.Value.Del(cellEntity);
            }
            if (multiplier != 1) Debug.Log($"Multiply: {multiplier}");
        }
    }
}