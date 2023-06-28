using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Temp.Components;
using Temp.Utils;
using Temp.Views;
using Temp.Views.Cell;
using UnityEngine;

namespace Temp.GameplaySystems.GridPowerUp
{
    public class ActivateCrossSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CellComponent, DestroyableCellStateComponent, CellPowerUpComponent>> _destroyablePowerUpCellsFilter = default;
        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;
        private readonly EcsFilterInject<Inc<OccupiedCellStateComponent, CellComponent>> _occupiedCellsFilter = default;
        private readonly EcsPoolInject<CellPowerUpComponent> _cellWithPowerUpsFilterPool = default;

        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, DraggingObjectComponent>> _draggingFigureFilter = default;
        private readonly EcsFilterInject<Inc<AnchoredBlockCellComponent>> _anchoredCellsFilter = default;

        private readonly EcsFilterInject<Inc<DestroyableCellStateComponent>> _destroyableCellsFilter = default;

        private readonly EcsPoolInject<OccupiedCellStateComponent> _occupiedCellsPool = default;
        private readonly EcsPoolInject<HighlightedCellStateComponent> _highlightedCellsPool = default;
        private readonly EcsPoolInject<DefaultCellStateComponent> _defaultCellsPool = default;

        private readonly Dictionary<Vector3Int, int> _entityCellsByPosition = new();
        private int _xOffset, _yOffset, _edge;
        private EcsWorld _world;
        private EcsPackedEntity? _previousAnchoredEntity;
        
        
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _previousAnchoredEntity = null;
            _entityCellsByPosition.Clear();
            foreach (var entity in _cellsFilter.Value)
            {
                ref var c = ref _cellsFilter.Pools.Inc1.Get(entity);
                _entityCellsByPosition.Add(c.Position.GetIntVector(), entity);
            }
            
            ref var firstCell = ref _cellsFilter.Pools.Inc1.Get(_cellsFilter.Value.GetRawEntities()[0]);
            _xOffset = (int) firstCell.View.ParentPosition.x;
            _yOffset = (int) firstCell.View.ParentPosition.y;
            _edge = Mathf.RoundToInt((float)Math.Sqrt(_cellsFilter.Value.GetEntitiesCount())) / 2;
        }

        public void Run(IEcsSystems systems)
        {
            if (_draggingFigureFilter.Value.GetEntitiesCount() == 0) return;
            //if (_destroyablePowerUpCellsFilter.Value.GetEntitiesCount() == 0) return;
            
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
            
            

            // var list = new List<Vector3Int>();
            //
            // foreach (var cellEntity in _destroyablePowerUpCellsFilter.Value)
            // {
            //     ref var cellPowerUp = ref _destroyablePowerUpCellsFilter.Pools.Inc3.Get(cellEntity);
            //     
            //     if (cellPowerUp.Type != PowerUpType.Cross) continue;
            //
            //     ref var cell = ref _destroyablePowerUpCellsFilter.Pools.Inc1.Get(cellEntity);
            //     
            //     list.Add(cell.Position.GetIntVector());
            //     //CompleteCellsCross(cell.Position.GetIntVector());
            // }
            //
            // foreach (var position in list)
            // {
            //     Activate(position);
            // }
        }

        // private void Activate(Vector3Int rootCellPosition)
        // {
        //     var queue = new Queue<Vector3Int>();
        //     queue.Enqueue(rootCellPosition);
        //     while (queue.Count > 0)
        //     {
        //         var pos = queue.Dequeue();
        //         
        //         for (var x = -_edge; x <= _edge; x += (int)CellView.Size)
        //         {
        //             var position = new Vector3Int(x + _xOffset, pos.y + _yOffset);
        //             if (!_entityCellsByPosition.TryGetValue(position, out var entity)) throw new Exception($"Can't reach cell by {position} position");
        //             
        //             if (_destroyablePowerUpCellsFilter.Pools.Inc3.Has(entity))
        //             {
        //                 ref var p = ref _destroyablePowerUpCellsFilter.Pools.Inc3.Get(entity);
        //                 if (p.Type != PowerUpType.Cross) continue;
        //                 ref var cell = ref _cellsFilter.Pools.Inc1.Get(entity);
        //                 queue.Enqueue(cell.Position.GetIntVector());
        //             }
        //         
        //             if (_highlightedCellsPool.Value.Has(entity) || _defaultCellsPool.Value.Has(entity) || _occupiedCellsPool.Value.Has(entity))
        //             {
        //                 var previousState = CellStateEnum.Highlighted;
        //                 if (_occupiedCellsPool.Value.Has(entity)) previousState = CellStateEnum.Occupied;
        //             
        //                 CellEntity.SetState(_world.PackEntityWithWorld(entity), CellStateEnum.Destroyable, previousState);
        //             }
        //         }
        //         
        //         for (var y = -_edge; y <= _edge; y += (int)CellView.Size)
        //         {
        //             var position = new Vector3Int(pos.x + _xOffset, y + _yOffset);
        //             if (!_entityCellsByPosition.TryGetValue(position, out var entity))
        //                 throw new Exception($"Can't reach cell by {position} position");
        //
        //             if (_destroyablePowerUpCellsFilter.Pools.Inc3.Has(entity))
        //             {
        //                 ref var p = ref _destroyablePowerUpCellsFilter.Pools.Inc3.Get(entity);
        //                 if (p.Type != PowerUpType.Cross) continue;
        //                 ref var cell = ref _cellsFilter.Pools.Inc1.Get(entity);
        //                 queue.Enqueue(cell.Position.GetIntVector());
        //             }
        //         
        //             if (_highlightedCellsPool.Value.Has(entity) || _defaultCellsPool.Value.Has(entity) || _occupiedCellsPool.Value.Has(entity))
        //             {
        //                 var previousState = CellStateEnum.Highlighted;
        //                 if (_occupiedCellsPool.Value.Has(entity)) previousState = CellStateEnum.Occupied;
        //             
        //                 CellEntity.SetState(_world.PackEntityWithWorld(entity), CellStateEnum.Destroyable, previousState);
        //             }
        //         }
        //     }
        // }
        
        /*
         * Node Find(Node root, int value)
         * {
                var queue = new Queue<Node>();
                queue.Enqueue(root);
                while (queue.Count > 0) 
                {
                    var node = queue.Dequeue();
                    if (node.value == value) return node;
                    for (int i = 0; i < node.children.Length; ++i) 
                    {
                        queue.Enqueue(node.children[i]);
                    }
                }
                return null;
            }
         */
    }
}