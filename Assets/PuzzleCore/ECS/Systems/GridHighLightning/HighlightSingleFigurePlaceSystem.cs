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

namespace PuzzleCore.ECS.Systems.GridHighLightning
{
    public class HighlightSingleFigurePlaceSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, AvailablePlacesComponent>> _figuresFilter = default;
        private readonly EcsPoolInject<CellComponent> _cellComponents = default;
        private readonly EcsPoolInject<PuzzleFigureComponent> _puzzleFigures = default;
        private readonly EcsPoolInject<AvailablePlacesComponent> _figurePlaceComponents = default;
        
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
            if (!_events.HasEventSingleton<CheckOnEndGameComponent>()) return;

            if (_figuresFilter.Value.GetEntitiesCount() == 0) return;

            var placeableFigures = 0;
            var entity = -1;
            foreach (var figureEntity in _figuresFilter.Value)
            {
                ref var val = ref _figurePlaceComponents.Value.Get(figureEntity);

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
            
            ref var figure = ref _puzzleFigures.Value.Get(entity);
            ref var placeData = ref _figurePlaceComponents.Value.Get(entity);

            var blockPositions = figure.RelativeBlockPositions;
            ref var cell = ref _cellComponents.Value.Get(placeData.AnchorCellEntity);
            cell.View.ChangeState(CellView.CellState.Suggested);
            //cell.View.SetSuggestion();
            var cellPosition = cell.Position.GetIntVector();
            for (var i = 0; i < blockPositions.Length; i++)
            {
                if (!_entityCellsByPosition.TryGetValue(cellPosition + blockPositions[i].GetIntVector(),
                        out var e)) continue;
                
                ref var c = ref _cellComponents.Value.Get(e);
                //c.View.SetSuggestion();
                c.View.ChangeState(CellView.CellState.Suggested);
            }
        }

        
    }
}