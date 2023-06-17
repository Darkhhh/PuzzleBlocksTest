using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Common;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using PuzzleCore.ECS.Views;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.Drag
{
    /// <summary>
    /// Определяет над какими клетками проходит перетаскиваемый над сеткой объект
    /// </summary>
    public class DragOverGridHandleSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region ECS Filters

        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;
        
        private readonly EcsFilterInject<Inc<AnchoredBlockCellComponent, CellComponent>> _anchoredCellsFilter = default;
        
        private readonly EcsFilterInject<Inc<CellOrderedForPlacementComponent, CellComponent>> _orderedCellsFilter = default;
        
        private readonly EcsFilterInject<Inc<DraggingObjectComponent, DraggingOverGridComponent>> _draggingObjectsFilter = default;

        #endregion
        
        
        #region ECS Pools

        private readonly EcsPoolInject<CellComponent> _cellComponents = default;
        
        private readonly EcsPoolInject<CellOrderedForPlacementComponent> _orderedCellComponents = default;
        
        private readonly EcsPoolInject<AnchoredBlockCellComponent> _anchorCellComponents = default;
        
        private readonly EcsPoolInject<DraggingOverGridComponent> _draggingOverGridObjectComponents = default;

        #endregion
        
        
        #region Private Values
        
        private readonly float _magnetDistance;
        
        private readonly Dictionary<Vector3Int, int> _entityCellsByPosition = new();

        private EventsBus _events;
        
        #endregion


        #region Init

        public DragOverGridHandleSystem(float magnetDistance)
        {
            _magnetDistance = magnetDistance;
        }
        
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
            foreach (var entity in _cellsFilter.Value)
            {
                ref var c = ref _cellComponents.Value.Get(entity);
                _entityCellsByPosition.Add(c.Position.GetIntVector(), entity);
            }
        }

        #endregion
        

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _draggingObjectsFilter.Value)
            {
                // Если есть хоть один перетаскиваемый над сеткой объект
                ref var dragOverGridObject = ref _draggingOverGridObjectComponents.Value.Get(entity);

                var anchorCellEntity = -1;
                foreach (var e in _cellsFilter.Value)
                {
                    ref var cell = ref _cellComponents.Value.Get(e);
                    
                    if(!(Utilities.CountDistanceNoSqrt(cell.Position, dragOverGridObject.CurrentPosition) < _magnetDistance * _magnetDistance)) continue;

                    
                    if (dragOverGridObject.CheckOnCellAvailability)
                    {
                        //TODO Check
                        if(systems.GetWorld().GetPool<OccupiedCellStateComponent>().Has(e)) continue;
                        
                        //if (!cell.Available) continue;
                        
                        anchorCellEntity = e;
                        break;
                    }
                    else
                    {
                        anchorCellEntity = e;
                        break;
                    }
                    // if (!cell.Available ||
                    //     !(Utilities.CountDistanceNoSqrt(cell.Position, dragOverGridObject.CurrentPosition)
                    //       < _magnetDistance * _magnetDistance)) continue;
                    // anchorCellEntity = e;
                    // break;
                }
                
                if (anchorCellEntity == -1)
                {
                    // Если есть перетаскиваемый над сеткой объект, но он находится вне сетки
                    
                    //ToDO !!!!! Поменять клетки на Default и закинуть компонент ChangeState
                    {
                        foreach (var eni in systems.GetWorld().Filter<HighlightedCellStateComponent>().End())
                        {
                            systems.GetWorld().GetPool<HighlightedCellStateComponent>().Del(eni);
                            systems.GetWorld().GetPool<DefaultCellStateComponent>().Add(eni);
                            systems.GetWorld().GetPool<ChangeCellStateComponent>().Add(eni);
                        }
                    }
                    //_events.NewEventSingleton<DeHighlightGridEvent>();
                    //ClearOrderedCells();
                    return;
                }


                if (_anchoredCellsFilter.Value.GetEntitiesCount() > 0 &&
                    _anchoredCellsFilter.Value.GetRawEntities()[0] == anchorCellEntity)
                {
                    // Если есть перетаскиваемый объект, но он не поменял свою позицию
                    return;
                }

                // Если есть перетаскиваемый объект, но он поменял свою позицию
                //_events.NewEventSingleton<HighlightGridEvent>();
                
                
                ref var anchorCell = ref _cellComponents.Value.Get(anchorCellEntity);

                var orderedCellsEntities = FindPlaceableObjectCells(
                        dragOverGridObject.PlaceableObject, 
                        anchorCell.Position.GetIntVector(), 
                        dragOverGridObject.CheckOnCellAvailability);

                if (dragOverGridObject.MustBeFullOnGrid &&
                    orderedCellsEntities.Count != dragOverGridObject.PlaceableObject.GetRelativeBlockPositions().Length)
                {
                    return;
                }

            
                //ClearOrderedCells();
            
                
                //ToDO !!!!!!!!!!!!!
                foreach (var orderedCellEntity in orderedCellsEntities)
                {
                    //_orderedCellComponents.Value.Add(orderedCellEntity);
                    systems.GetWorld().GetPool<HighlightedCellStateComponent>().Add(orderedCellEntity);
                }
                systems.GetWorld().GetPool<HighlightedCellStateComponent>().Add(anchorCellEntity);
                //_orderedCellComponents.Value.Add(anchorCellEntity);
                _anchorCellComponents.Value.Add(anchorCellEntity);
                
                // !!!!!!!!!!!!!
            }
        }


        private readonly EcsPoolInject<OccupiedCellStateComponent> _occupiedCellsComponents = default;
        
        #region Private Methods

        private List<int> FindPlaceableObjectCells(IGridPlaceableObject placeableObject, Vector3Int anchorCellPosition,  
            bool checkOnAvailable = true)
        {
            var blockPositions = placeableObject.GetRelativeBlockPositions();
            var orderedCellsEntities = new List<int>(blockPositions.Length);
            
            foreach (var blockPosition in blockPositions)
            {
                if (!_entityCellsByPosition.TryGetValue(anchorCellPosition + blockPosition, out var entity)) continue;
                if (checkOnAvailable)
                {
                    ref var c = ref _cellComponents.Value.Get(entity);
                    
                    //TODO Check
                    if(_occupiedCellsComponents.Value.Has(entity)) orderedCellsEntities.Add(entity);
                    //if (c.Available) orderedCellsEntities.Add(entity);
                }
                else
                {
                    orderedCellsEntities.Add(entity);
                }
            }

            return orderedCellsEntities;
        }
        
        
        private void ClearOrderedCells()
        {
            // TODO Remove
            // foreach (var entity in _orderedCellsFilter.Value)
            // {
            //     _orderedCellComponents.Value.Del(entity);
            // }
            //
            // foreach (var entity in _anchoredCellsFilter.Value)
            // {
            //     _anchorCellComponents.Value.Del(entity);
            // }
        }

        #endregion
    }
}