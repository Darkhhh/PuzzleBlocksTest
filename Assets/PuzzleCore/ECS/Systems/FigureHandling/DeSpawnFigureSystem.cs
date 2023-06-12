using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;

namespace PuzzleCore.ECS.Systems.FigureHandling
{
    public class DeSpawnFigureSystem : IEcsRunSystem
    {
        #region ECS Filters

        private readonly EcsFilterInject<Inc<ShouldBeRemovedFigureComponent, PuzzleFigureComponent>> _removingFiguresFilter = default;

        #endregion


        #region ECS Pools

        private readonly EcsPoolInject<DraggingFigureComponent> _draggingFiguresComponents = default;
        
        private readonly EcsPoolInject<ShouldBeRemovedFigureComponent> _removingPuzzleFigures = default;
        
        private readonly EcsPoolInject<PuzzleFigureComponent> _puzzleFigures = default;
        
        private readonly EcsPoolInject<DraggableObjectComponent> _draggableObjectComponents = default;
        
        private readonly EcsPoolInject<DraggableOverGridComponent> _draggableOverGridObjectComponents = default;
        
        private readonly EcsPoolInject<AvailablePlacesComponent> _figurePlaceComponents = default;

        #endregion
        
        
        public void Run(IEcsSystems systems)
        {
            if (_removingFiguresFilter.Value.GetEntitiesCount() == 0) return;

            foreach (var entity in _removingFiguresFilter.Value)
            {
                ref var puzzleFigure = ref _puzzleFigures.Value.Get(entity);
                
                
                puzzleFigure.View.gameObject.SetActive(false);
                //Object.Destroy(puzzleFigure.View.gameObject);
                
                
                _puzzleFigures.Value.Del(entity);
                _draggingFiguresComponents.Value.Del(entity);
                _removingPuzzleFigures.Value.Del(entity);
                _draggableObjectComponents.Value.Del(entity);
                _draggableOverGridObjectComponents.Value.Del(entity);
                _figurePlaceComponents.Value.Del(entity);
            }
            
            var events = systems.GetShared<SystemsSharedData>().EventsBus;
            events.NewEventSingleton<CheckOnEndGameComponent>();
        }
    }
}