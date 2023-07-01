using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.Components;
using Source.Code.Utils;
using Source.Code.Views;
using Source.Code.Views.Cell;
using UnityEngine;

namespace Source.Code.GameplaySystems.GridPowerUp
{
    public class ActivateCrossSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CellComponent, DestroyableCellStateComponent, CellPowerUpComponent>> _destroyablePowerUpCellsFilter = default;
        private readonly EcsFilterInject<Inc<OccupiedCellStateComponent, CellComponent>> _occupiedCellsFilter = default;
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, DraggingObjectComponent>> _draggingFigureFilter = default;
        private readonly EcsFilterInject<Inc<AnchoredBlockCellComponent>> _anchoredCellsFilter = default;
        
        private readonly EcsPoolInject<CellPowerUpComponent> _cellWithPowerUpsFilterPool = default;
        
        private EcsWorld _world;
        private EcsPackedEntity? _previousAnchoredEntity;
        
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _previousAnchoredEntity = null;
        }

        
        public void Run(IEcsSystems systems)
        {
            if (_draggingFigureFilter.Value.GetEntitiesCount() == 0) return;
            
            // Если нет якорных клеток, значит фигура либо вне доски, либо не может быть установлена
            if (_anchoredCellsFilter.Value.GetEntitiesCount() == 0)
            {
                _previousAnchoredEntity = null;
                return;
            }

            var unpackedAnchorEntity = -1;
            _previousAnchoredEntity?.Unpack(_world, out unpackedAnchorEntity);
            
            
            
            // Если якорная клетка та же, то значит фигура место не меняла
            foreach (var anchorEntity in _anchoredCellsFilter.Value)
            {
                if (anchorEntity == unpackedAnchorEntity) return;

                _previousAnchoredEntity = _world.PackEntity(anchorEntity);
                break;
            }

            var q = new Queue<Vector3Int>();
            foreach (var cellEntity in _destroyablePowerUpCellsFilter.Value)
            {
                ref var cellPowerUp = ref _destroyablePowerUpCellsFilter.Pools.Inc3.Get(cellEntity);
                if (cellPowerUp.Type != PowerUpType.Cross) continue;
                ref var cell = ref _destroyablePowerUpCellsFilter.Pools.Inc1.Get(cellEntity);
                q.Enqueue(cell.Position.GetIntVector());
            }

            while (q.Count > 0)
            {
                var item = q.Dequeue();

                foreach (var cellEntity in _occupiedCellsFilter.Value)
                {
                    ref var cell = ref _occupiedCellsFilter.Pools.Inc2.Get(cellEntity);
                    var intPosition = cell.Position.GetIntVector();

                    if (intPosition.x != item.x && intPosition.y != item.y) continue;
                    
                    if (_cellWithPowerUpsFilterPool.Value.Has(cellEntity))
                    {
                        ref var cellPowerUp = ref _cellWithPowerUpsFilterPool.Value.Get(cellEntity);
                        if (cellPowerUp.Type == PowerUpType.Cross) q.Enqueue(cell.Position.GetIntVector());
                    }
                    
                    CellEntity.SetState(_world.PackEntityWithWorld(cellEntity), CellStateEnum.Destroyable, CellStateEnum.Occupied);
                }
            }
        }
    }
}