using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Common.Utils;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.SharedData;
using UnityEngine;

namespace Source.Code.PreGameplayRunSystems
{
    /// <summary>
    /// Для каждой активной фигуры находит возможное место расположения, если место не найдено, то
    /// она становится CanNotBeTakenComponent, иначе, на фигуре появляется компонент AvailablePlacesComponent
    /// </summary>
    public class FindPlaceForFiguresSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent>> _figuresFilter = default;
        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;

        private readonly EcsPoolInject<PuzzleFigureComponent> _figuresPool = default;
        private readonly EcsPoolInject<AvailablePlacesComponent> _availablePlacesPool = default;
        private readonly EcsPoolInject<CanNotBeTakenComponent> _notTakenFiguresPool = default;
        private readonly EcsPoolInject<CellComponent> _cellComponents = default;
        private readonly EcsPoolInject<OccupiedCellStateComponent> _occupiedCellsPool = default;

        private readonly Dictionary<Vector3Int, int> _entityCellsByPosition = new();
        private EventsBus _events;
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
            foreach (var entity in _cellsFilter.Value)
            {
                ref var c = ref _cellComponents.Value.Get(entity);
                _entityCellsByPosition.Add(c.Position.GetIntVector(), entity);
            }
        }
        

        public void Run(IEcsSystems systems)
        {
            // Если для фигуры нет места, навесить на нее CanNotBeTakenComponent
            // Если для фигуры есть место, то навесить AvailablePlacesComponent

            if (_figuresFilter.Value.GetEntitiesCount() == 0) return;

            if (!_events.HasEventSingleton<FindPlaceForFiguresEvent>()) return;

            foreach (var entity in _figuresFilter.Value)
            {
                if (_availablePlacesPool.Value.Has(entity)) _availablePlacesPool.Value.Del(entity);
                if (_notTakenFiguresPool.Value.Has(entity)) _notTakenFiguresPool.Value.Del(entity);
                
                ref var component = ref _availablePlacesPool.Value.Add(entity);
                ref var figure = ref _figuresPool.Value.Get(entity);
                figure.View.SetToDefault();
                
                var places = 0;

                var flag = false;
                foreach (var cellEntity in _cellsFilter.Value)
                {
                    ref var cell = ref _cellComponents.Value.Get(cellEntity);
                    
                    if (_occupiedCellsPool.Value.Has(cellEntity)) continue;
                    if (!FigureCanBePlaced(cell.Position.GetIntVector(), figure.RelativeBlockPositions)) continue;
                    

                    places++;
                    component.Amount = places;
                    component.AnchorCellEntity = cellEntity;

                    if (flag) break;
                    flag = true;
                }

                if (flag) continue;
                
                _availablePlacesPool.Value.Del(entity);
                _notTakenFiguresPool.Value.Add(entity);
                figure.View.SetToUntouchable();
            }
            
            _events.DestroyEventSingleton<FindPlaceForFiguresEvent>();
        }
        
        
        private bool FigureCanBePlaced(Vector3Int cellPosition, Vector3[] blockPositions)
        {
            for (var i = 0; i < blockPositions.Length; i++)
            {
                if (!_entityCellsByPosition.TryGetValue(cellPosition + blockPositions[i].GetIntVector(),
                        out var entity)) return false;
                if (_occupiedCellsPool.Value.Has(entity)) return false;
            }

            return true;
        }
    }
}