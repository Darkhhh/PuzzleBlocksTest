using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.Components;
using Source.Code.Utils;
using Source.Code.Views.Cell;
using UnityEngine;

namespace Source.Code.GameplaySystems
{
    public class SuggestCellsForFigureSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, AvailablePlacesComponent>> _figuresFilter = default;
        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;
        private readonly EcsFilterInject<Inc<SuggestedCellStateComponent>> _suggestedCellsFilter = default;

        private readonly EcsPoolInject<PuzzleFigureComponent> _figuresPool = default;
        private readonly EcsPoolInject<AvailablePlacesComponent> _availablePlacesPool = default;
        private readonly EcsPoolInject<HighlightedCellStateComponent> _highlightedCellsPool = default;
        
        private readonly Dictionary<Vector3Int, int> _entityCellsByPosition = new();
        
        
        public void Init(IEcsSystems systems)
        {
            foreach (var entity in _cellsFilter.Value)
            {
                ref var c = ref _cellsFilter.Pools.Inc1.Get(entity);
                _entityCellsByPosition.Add(c.Position.GetIntVector(), entity);
            }
        }
        
        
        public void Run(IEcsSystems systems)
        {
            //TODO Change Return conditions
            //if (!_events.HasEventSingleton<FiguresWereSpawnedEvent>()) return;

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
            if (_suggestedCellsFilter.Value.GetEntitiesCount() > 0) return;
            
            ref var figure = ref _figuresPool.Value.Get(entity);
            ref var placeData = ref _availablePlacesPool.Value.Get(entity);

            var blockPositions = figure.RelativeBlockPositions;
            ref var cell = ref _cellsFilter.Pools.Inc1.Get(placeData.AnchorCellEntity);
            
            var cellPosition = cell.Position.GetIntVector();
            for (var i = 0; i < blockPositions.Length; i++)
            {
                if (!_entityCellsByPosition.TryGetValue(cellPosition + blockPositions[i].GetIntVector(),
                        out var e)) continue;
                if (!_highlightedCellsPool.Value.Has(e))
                    CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(e), CellStateEnum.Suggested);
            }
            if (!_highlightedCellsPool.Value.Has(placeData.AnchorCellEntity))
                CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(placeData.AnchorCellEntity), CellStateEnum.Suggested);
        }
    }
}