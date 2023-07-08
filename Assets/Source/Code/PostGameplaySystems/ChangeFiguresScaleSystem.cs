using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.SharedData;
using UnityEngine;

namespace Source.Code.PostGameplaySystems
{
    public class ChangeFiguresScaleSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region Scales

        private readonly Vector3 _defaultScale = new (1,1,1);
        private readonly Vector3 _spawnScale = new (0.6f, 0.6f, 0.6f);
        private readonly Vector3 _draggingScale = new (0.9f, 0.9f, 0.9f);

        #endregion
        
        
        #region ECS Filters

        private readonly EcsFilterInject<Inc<PuzzleFigureComponent>> _puzzleFiguresFilter = default;
        private readonly EcsFilterInject<Inc<ShouldBeRemovedFigureComponent>> _removingFiguresFilter = default;

        #endregion


        #region ECS Pools

        private readonly EcsPoolInject<PuzzleFigureComponent> _puzzleFigureComponents = default;

        #endregion
        
        private EventsBus _events;
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }

        public void Run(IEcsSystems systems)
        {
            
            if (_events.HasEventSingleton<FiguresWereSpawnedEvent>()) DecreaseScaleToAllFigures();

            if (_events.HasEventSingleton<ChangeFigureScaleComponent>())
            {
                ref var e = ref _events.GetEventBodySingleton<ChangeFigureScaleComponent>();
                if (e.Increase) IncreaseScale(e.Entity);
                else DecreaseScale(e.Entity);
                
                _events.DestroyEventSingleton<ChangeFigureScaleComponent>();
            }

            if (_removingFiguresFilter.Value.GetEntitiesCount() != 0)
            {
                foreach (var entity in _removingFiguresFilter.Value) SetDefaultScale(entity);
            }
        }

        
        private void DecreaseScaleToAllFigures()
        {
            foreach (var figureEntity in _puzzleFiguresFilter.Value) DecreaseScale(figureEntity);
        }

        
        private void DecreaseScale(int figureEntity)
        {
            ref var figure = ref _puzzleFigureComponents.Value.Get(figureEntity);

            var transform = figure.View.transform;
            var localScale = transform.localScale;
            var scaleVector = _spawnScale - localScale;
            
            localScale += scaleVector;
            transform.localScale = localScale;
        }

        
        private void IncreaseScale(int figureEntity)
        {
            ref var figure = ref _puzzleFigureComponents.Value.Get(figureEntity);
            var transform = figure.View.transform;
            var localScale = transform.localScale;
            var scaleVector = _draggingScale - localScale;
            
            localScale += scaleVector;
            transform.localScale = localScale;
        }

        private void SetDefaultScale(int figureEntity)
        {
            ref var figure = ref _puzzleFigureComponents.Value.Get(figureEntity);
            var transform = figure.View.transform;
            var localScale = transform.localScale;
            var scaleVector = _defaultScale - localScale;
            
            localScale += scaleVector;
            transform.localScale = localScale;
        }
    }
}