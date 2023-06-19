using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Common;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;
using Temp.Components.Events;
using UnityEngine;

namespace Temp.GameplaySystems
{
    public class SuggestCellsForFigureSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, AvailablePlacesComponent>> _figuresFilter = default;
        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;

        private readonly EcsPoolInject<PuzzleFigureComponent> _figuresPool = default;
        private readonly EcsPoolInject<AvailablePlacesComponent> _availablePlacesPool = default;
        private readonly EcsPoolInject<CanNotBeTakenComponent> _notTakenFiguresPool = default;
        private readonly EcsPoolInject<CellComponent> _cellComponents = default;
        private readonly EcsPoolInject<DefaultCellStateComponent> _defaultCellsPool = default;
        private readonly EcsPoolInject<SuggestedCellStateComponent> _suggestedCellsPool = default;
        private readonly EcsPoolInject<ChangeCellStateComponent> _changeStateCellsPool = default;


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
            if (!_events.HasEventSingleton<FiguresWereSpawnedEvent>()) return;

            if (_figuresFilter.Value.GetEntitiesCount() == 0) return;

            var placeableFigures = 0;
            var entity = -1;
            foreach (var figureEntity in _figuresFilter.Value)
            {
                ref var val = ref _availablePlacesPool.Value.Get(figureEntity);

                switch (val.Amount)
                {
                    case > 1:
                        return;
                    case 0:
                        continue;
                    default:
                        placeableFigures++;
                        entity = figureEntity;
                        break;
                }
            }

            if (placeableFigures != 1) return;
            
            ref var figure = ref _figuresPool.Value.Get(entity);
            ref var placeData = ref _availablePlacesPool.Value.Get(entity);

            var blockPositions = figure.RelativeBlockPositions;
            ref var cell = ref _cellComponents.Value.Get(placeData.AnchorCellEntity);
            
            var cellPosition = cell.Position.GetIntVector();
            for (var i = 0; i < blockPositions.Length; i++)
            {
                if (!_entityCellsByPosition.TryGetValue(cellPosition + blockPositions[i].GetIntVector(),
                        out var e)) continue;
                
                _suggestedCellsPool.Value.Add(e);
                if (!_changeStateCellsPool.Value.Has(e))
                    _changeStateCellsPool.Value.Add(e);
                if (_defaultCellsPool.Value.Has(e))
                    _defaultCellsPool.Value.Add(e);
            }

            
            _suggestedCellsPool.Value.Add(placeData.AnchorCellEntity);
            if (!_changeStateCellsPool.Value.Has(placeData.AnchorCellEntity))
                _changeStateCellsPool.Value.Add(placeData.AnchorCellEntity);
            if (_defaultCellsPool.Value.Has(placeData.AnchorCellEntity))
                _defaultCellsPool.Value.Add(placeData.AnchorCellEntity);
        }
    }
}