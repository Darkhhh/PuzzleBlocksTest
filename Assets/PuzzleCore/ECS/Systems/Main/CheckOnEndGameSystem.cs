using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Common;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.Main
{
    public class CheckOnEndGameSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly Dictionary<Vector3Int, int> _entityCellsByPosition = new();
        private EventsBus _events;
        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent>> _figuresFilter = default;
        private readonly EcsPoolInject<CellComponent> _cellComponents = default;
        private readonly EcsPoolInject<PuzzleFigureComponent> _puzzleFigures = default;
        private readonly EcsPoolInject<AvailablePlacesComponent> _figurePlaceComponents = default;
        
        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<CheckOnEndGameComponent>()) return;

            if (_figuresFilter.Value.GetEntitiesCount() == 0) return;
            
            var placeableFigures = 0;
            foreach (var figureEntity in _figuresFilter.Value)
            {
                ref var figure = ref _puzzleFigures.Value.Get(figureEntity);
                
                // EXPERIMENTAL
                if (!_figurePlaceComponents.Value.Has(figureEntity)) _figurePlaceComponents.Value.Add(figureEntity);
                var places = 0;
                // END EXPERIMENTAL
                
                foreach (var entity in _cellsFilter.Value)
                {
                    ref var cell = ref _cellComponents.Value.Get(entity);
                    
                    if (!cell.Available) continue;
                    if (!FigureCanBePlaced(cell.Position.GetIntVector(), figure.RelativeBlockPositions)) continue;

                    placeableFigures++;

                    // EXPERIMENTAL
                    places++;
                    ref var t = ref _figurePlaceComponents.Value.Get(figureEntity);
                    t.Amount = places;
                    t.AnchorCellEntity = entity;
                    // END EXPERIMENTAL
                    
                }
            }
            
            if (placeableFigures == 0) Debug.Log("End Game!");
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
        
        
        private bool FigureCanBePlaced(Vector3Int cellPosition, Vector3[] blockPositions)
        {
            for (var i = 0; i < blockPositions.Length; i++)
            {
                if (!_entityCellsByPosition.TryGetValue(cellPosition + blockPositions[i].GetIntVector(),
                        out var entity)) return false;
                
                ref var c = ref _cellComponents.Value.Get(entity);
                if (!c.Available) return false;
            }

            return true;
        }
    }
}