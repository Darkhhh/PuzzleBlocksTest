using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Common.Animations;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.SharedData;
using UnityEngine;

namespace Source.Code.AnimationSystems
{
    public class SwapFiguresAndPowerUpsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const int XOffset = 10;
        private static readonly Vector3 Offset = new (XOffset, 0);
        private const float ChangingSpeed = 7;


        private readonly EcsFilterInject<Inc<PuzzleFigureComponent>> _figuresFilter = default;
        private readonly EcsFilterInject<Inc<ManualPowerUp>> _manualPowerUpFilter = default;

        private readonly EcsPoolInject<DoNotTakeObject> _doNotTakeObjectPool = default;

        private bool _figuresActive;

        private Vector3[] _manualPowerUpsHidePositions, _manualPowerUpsShowPositions;
        private Vector3[] _puzzleFigureHidePositions, _puzzleFigureShowPositions;
        private EventsBus _events;


        private Vector3 _figuresHidePosition = new(-6, -7), _powerUpsHidePosition = new (6, -7);
        private int _movingObjects;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
            var powerUpsPool = world.GetPool<ManualPowerUp>();
            var figuresPool = world.GetPool<PuzzleFigureComponent>();

            var powerUpsFilter = world.Filter<ManualPowerUp>().End();
            var figuresFilter = world.Filter<PuzzleFigureComponent>().End();

            _manualPowerUpsShowPositions = new Vector3[powerUpsFilter.GetEntitiesCount()];
            var index = 0;
            foreach (var powerUpEntity in powerUpsFilter)
            {
                ref var powerUp = ref powerUpsPool.Get(powerUpEntity);
                
                if (!_doNotTakeObjectPool.Value.Has(powerUpEntity)) _doNotTakeObjectPool.Value.Add(powerUpEntity);

                var transform = powerUp.View.transform;
                var position = transform.position;
                
                _manualPowerUpsShowPositions[index] = position;
                index++;
                position += Offset;
                transform.position = position;
            }

            _figuresActive = true;

            index = 0;
            
            _puzzleFigureShowPositions = new Vector3[figuresFilter.GetEntitiesCount()];
            
            foreach (var figureEntity in figuresFilter)
            {
                ref var figure = ref figuresPool.Get(figureEntity);

                var transform = figure.View.transform;
                var position = transform.position;
                
                _puzzleFigureShowPositions[index] = position;
                index++;
            }
        }

        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<SwapFiguresAndPowerUpsEvent>()) return;
            _events.DestroyEventSingleton<SwapFiguresAndPowerUpsEvent>();
            if (_movingObjects > 0) return;
            
            
            Vector3 offset;
            if (_figuresActive) offset = Vector3.zero - Offset;
            else offset = Offset;
            _figuresActive = !_figuresActive;

            _movingObjects = _figuresFilter.Value.GetEntitiesCount() + _manualPowerUpFilter.Value.GetEntitiesCount();
            
            foreach (var figureEntity in _figuresFilter.Value)
            {
                ref var figure = ref _figuresFilter.Pools.Inc1.Get(figureEntity);

                if (!_doNotTakeObjectPool.Value.Has(figureEntity)) _doNotTakeObjectPool.Value.Add(figureEntity);

                var transform = figure.View.transform;
                figure.View.ExecuteCoroutine(MovingCoroutines.MoveTowards(
                    transform, 
                    new Vector3(transform.position.x + offset.x, transform.position.y), 
                    ChangingSpeed, () =>
                    {
                        _movingObjects--;
                        
                        if (_figuresActive) _doNotTakeObjectPool.Value.Del(figureEntity);
                    }));
            }
            
            foreach (var powerUpEntity in _manualPowerUpFilter.Value)
            {
                ref var powerUp = ref _manualPowerUpFilter.Pools.Inc1.Get(powerUpEntity);
                powerUp.View.SetActiveCanvas(false);

                if (!_doNotTakeObjectPool.Value.Has(powerUpEntity)) _doNotTakeObjectPool.Value.Add(powerUpEntity);
                
                var transform = powerUp.View.transform;
                powerUp.View.ExecuteCoroutine(MovingCoroutines.MoveTowards(
                    transform, 
                    new Vector3(transform.position.x + offset.x, transform.position.y), 
                    ChangingSpeed,() =>
                    {
                        _movingObjects--;
                        if (!_figuresActive)
                        {
                            ref var powerUp = ref _manualPowerUpFilter.Pools.Inc1.Get(powerUpEntity);
                            powerUp.View.SetActiveCanvas(true);
                            _doNotTakeObjectPool.Value.Del(powerUpEntity);
                        }
                    }));
            }
        }
    }
}