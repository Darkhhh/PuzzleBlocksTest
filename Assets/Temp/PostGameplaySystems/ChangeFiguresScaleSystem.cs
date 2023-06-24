using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;
using Temp.Components.Events;
using UnityEngine;

namespace Temp.PostGameplaySystems
{
    public class ChangeFiguresScaleSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region Const

        private readonly Vector3 _defaultScale = new (1,1,1);
        private readonly Vector3 _spawnScale = new (0.7f, 0.7f, 0.7f);
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

            //figure.View.transform.DOScale(_spawnScale, 0.5f);
            var transform = figure.View.transform;
            var localScale = transform.localScale;
            var scaleVector = _spawnScale - localScale;
            
            localScale += scaleVector;
            transform.localScale = localScale;
        }

        
        private void IncreaseScale(int figureEntity)
        {
            ref var figure = ref _puzzleFigureComponents.Value.Get(figureEntity);
            //figure.View.transform.DOScale(_draggingScale, 0.5f);
            var transform = figure.View.transform;
            var localScale = transform.localScale;
            var scaleVector = _draggingScale - localScale;
            
            localScale += scaleVector;
            transform.localScale = localScale;
        }

        private void SetDefaultScale(int figureEntity)
        {
            ref var figure = ref _puzzleFigureComponents.Value.Get(figureEntity);
            //figure.View.transform.DOScale(_defaultScale, 0.01f);
            var transform = figure.View.transform;
            var localScale = transform.localScale;
            var scaleVector = _defaultScale - localScale;
            
            localScale += scaleVector;
            transform.localScale = localScale;
        }
    }
}