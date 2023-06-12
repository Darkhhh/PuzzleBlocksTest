using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.FigureHandling
{
    public class ChangeFigureScaleSystem: IEcsRunSystem
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

        
        public void Run(IEcsSystems systems)
        {
            var events = systems.GetShared<SystemsSharedData>().EventsBus;
            
            if (events.HasEventSingleton<DecreaseAllFiguresScaleComponent>()) DecreaseScaleToAllFigures();

            if (events.HasEventSingleton<ChangeFigureScaleComponent>())
            {
                ref var e = ref events.GetEventBodySingleton<ChangeFigureScaleComponent>();
                if (e.Increase) IncreaseScale(e.Entity);
                else DecreaseScale(e.Entity);
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