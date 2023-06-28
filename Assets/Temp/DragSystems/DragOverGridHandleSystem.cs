using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Temp.Components;
using Temp.SharedData;
using Temp.Utils;
using Temp.Views;
using Temp.Views.Cell;
using UnityEngine;

namespace Temp.DragSystems
{
    /// <summary>
    /// Определяет над какими клетками проходит перетаскиваемый над сеткой объект
    /// </summary>
    public class DragOverGridHandleSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region ECS Filters

        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;
        
        private readonly EcsFilterInject<Inc<AnchoredBlockCellComponent, CellComponent>> _anchoredCellsFilter = default;
        
        private readonly EcsFilterInject<Inc<DraggingObjectComponent, DraggingOverGridComponent>> _draggingObjectsFilter = default;

        #endregion
        
        
        #region ECS Pools

        private readonly EcsPoolInject<CellComponent> _cellComponents = default;
        
        private readonly EcsPoolInject<AnchoredBlockCellComponent> _anchorCellComponents = default;
        
        private readonly EcsPoolInject<DraggingOverGridComponent> _draggingOverGridObjectComponents = default;

        private readonly EcsPoolInject<OccupiedCellStateComponent> _occupiedCellsPool = default;

        private readonly EcsPoolInject<HighlightedCellStateComponent> _highlightedCellsPool = default;
        
        private readonly EcsPoolInject<SuggestedCellStateComponent> _suggestedCellsPool = default;
        
        private readonly EcsPoolInject<DefaultCellStateComponent> _defaultCellsPool = default;
        
        private readonly EcsPoolInject<DestroyableCellStateComponent> _destroyableCellsPool = default;
        
        private readonly EcsPoolInject<ChangeCellStateComponent> _changeStateCellsPool = default;

        #endregion
        
        
        #region Private Values
        
        private readonly Dictionary<Vector3Int, int> _entityCellsByPosition = new();

        private EventsBus _events;
        
        #endregion


        #region Init
        
        
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
                    
                    if(!(Utilities.CountDistanceNoSqrt(cell.Position, dragOverGridObject.CurrentPosition) < ConstantData.MagnetDistance * ConstantData.MagnetDistance)) continue;

                    
                    if (dragOverGridObject.CheckOnCellAvailability)
                    {
                        if(_occupiedCellsPool.Value.Has(e)) continue;
                        anchorCellEntity = e;
                        break;
                    }
                    else
                    {
                        anchorCellEntity = e;
                        break;
                    }
                }
                
                if (anchorCellEntity == -1)
                {
                    ClearAnchoredCells();
                    return;
                }


                if (_anchoredCellsFilter.Value.GetEntitiesCount() > 0 &&
                    _anchoredCellsFilter.Value.GetRawEntities()[0] == anchorCellEntity)
                {
                    // Если есть перетаскиваемый объект, но он не поменял свою позицию
                    return;
                }

                
                // Если есть перетаскиваемый объект, но он поменял свою позицию
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

                ClearAnchoredCells();
                
                foreach (var orderedCellEntity in orderedCellsEntities)
                {
                    if (_occupiedCellsPool.Value.Has(orderedCellEntity))
                    {
                        CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(orderedCellEntity),
                            CellStateEnum.Highlighted, CellStateEnum.Occupied);
                    }
                    else
                    {
                        CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(orderedCellEntity),
                            CellStateEnum.Highlighted);
                    }
                }
                
                if (_occupiedCellsPool.Value.Has(anchorCellEntity))
                {
                    CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(anchorCellEntity),
                        CellStateEnum.Highlighted, CellStateEnum.Occupied);
                }
                else
                {
                    CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(anchorCellEntity),
                        CellStateEnum.Highlighted);
                }
                _anchorCellComponents.Value.Add(anchorCellEntity);
            }
        }
        
        
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
                    if(!_occupiedCellsPool.Value.Has(entity) && !_destroyableCellsPool.Value.Has(entity)) 
                        orderedCellsEntities.Add(entity);
                }
                else
                {
                    orderedCellsEntities.Add(entity);
                }
            }

            return orderedCellsEntities;
        }
        
        private void ClearAnchoredCells()
        {
            foreach (var entity in _anchoredCellsFilter.Value)
            {
                _anchorCellComponents.Value.Del(entity);
            }
        }

        #endregion
    }
}