using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using PuzzleCore.ECS.Views;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.FigureHandling
{
    public class ReleaseFigureSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region ECS Filters
        
        private readonly EcsFilterInject<Inc<CellComponent, CellOrderedForPlacementComponent>> _orderedCellsFilter = default;
        
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ReleasedObjectComponent>> _releasedFiguresFilter =
            default;
        #endregion
        
        
        #region ECS Pools

        private readonly EcsPoolInject<CellComponent> _cellComponents = default;
        
        private readonly EcsPoolInject<ReleasedObjectComponent> _draggingFiguresComponents = default;
        
        private readonly EcsPoolInject<PuzzleFigureComponent> _puzzleFiguresComponents = default;
        
        private readonly EcsPoolInject<ShouldBeRemovedFigureComponent> _removedFiguresComponents = default;

        #endregion

        
        private EventsBus _events;
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _releasedFiguresFilter.Value)
            {
                ref var puzzleFigure = ref _puzzleFiguresComponents.Value.Get(entity);

                if (_orderedCellsFilter.Value.GetEntitiesCount() != puzzleFigure.RelativeBlockPositions.Length + 1)
                {
                    ref var draggingInfo = ref _draggingFiguresComponents.Value.Get(entity);
                    puzzleFigure.View.transform.DOMove(draggingInfo.InitialPosition - puzzleFigure.View.Offset,
                        0.5f);
                    //puzzleFigure.View.SetPositionByCenter(draggingInfo.InitialPosition);

                    _draggingFiguresComponents.Value.Del(entity);
                
                    ref var et = ref _events.NewEventSingleton<ChangeFigureScaleComponent>();
                    et.Entity = entity;
                    et.Increase = false;
                    return;
                }

                foreach (var ey in _orderedCellsFilter.Value)
                {
                    ref var cell = ref _cellComponents.Value.Get(ey);
                    //TODO Experiments
                    //cell.View.ChangeState(CellState.Occupied);
                    //cell.View.SetUnAvailable();
                    //cell.Available = false;
                }

                _removedFiguresComponents.Value.Add(entity);
                _draggingFiguresComponents.Value.Del(entity);
            
                ref var e = ref _events.NewEventSingleton<ChangeFigureScaleComponent>();
                e.Entity = entity;
                e.Increase = true;
            }
        }
    }
}