using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Temp.Components;
using Temp.Components.Events;
using Temp.Mono;
using Temp.SharedData;
using Temp.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Temp.PreGameplayRunSystems
{
    public class SpawnFiguresSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region ECS Filters

        private readonly EcsFilterInject<Inc<PuzzleFigureComponent>> _puzzleFiguresFilter = default;

        #endregion


        #region ECS Pools

        private readonly EcsPoolInject<PuzzleFigureComponent> _puzzleFigureComponents = default;
        private readonly EcsPoolInject<DraggableObjectComponent> _draggableObjectComponents = default;
        private readonly EcsPoolInject<DraggableOverGridComponent> _draggableOverGridObjectComponents = default;

        #endregion
        
        
        #region Private Values

        private readonly PuzzleFigureView[] _sceneFigures;
        
        private readonly Transform[] _spawnPoints;
        
        private float[] _figureWeights;
        
        private float _allWeight;

        private readonly List<int> _indices = new();

        private EcsWorld _world;

        #endregion


        #region Initialization

        public SpawnFiguresSystem(PuzzleFiguresHandler handler, Transform[] spawnPositions)
        {
            _sceneFigures = handler.GetScenePuzzleFigures();
            _spawnPoints = spawnPositions;
        }
        
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            // Counting Weights
            _allWeight = _sceneFigures.Sum(puzzleFigure => puzzleFigure.Weight);
            _figureWeights = new float[_sceneFigures.Length + 1];
            _figureWeights[0] = 0;
            for (var i = 1; i < _sceneFigures.Length + 1; i++)
            {
                _figureWeights[i] = _figureWeights[i - 1] + _sceneFigures[i - 1].Weight;
            }
        }

        #endregion
        

        public void Run(IEcsSystems systems)
        {
            if (_puzzleFiguresFilter.Value.GetEntitiesCount() != 0) return;
            _indices.Clear();

#if UNITY_EDITOR
            if (_sceneFigures.Any(figure => figure.gameObject.activeSelf))
                throw new Exception("Creating new figures: not all figures are disabled");
#endif
        
            foreach (var t in _spawnPoints)
            {
                var index = GetNewSpawnFigureIndex();
                while (true)
                {
                    if (!_indices.Contains(index)) break;
                    index = GetNewSpawnFigureIndex();
                }
                _indices.Add(index);

                var entity = _world.NewEntity();
                ref var puzzleFigure = ref _puzzleFigureComponents.Value.Add(entity);
                puzzleFigure.View = _sceneFigures[index];
                puzzleFigure.View.gameObject.SetActive(true);
                puzzleFigure.View.SetPositionByCenter(t.position);
                
                ref var draggableObject = ref _draggableObjectComponents.Value.Add(entity);
                draggableObject.View = puzzleFigure.View;

                ref var draggableOverGridObject = ref _draggableOverGridObjectComponents.Value.Add(entity);
                draggableOverGridObject.MustBeFullOnGrid = true;
                draggableOverGridObject.CheckOnCellAvailability = true;
                draggableOverGridObject.PlaceableObject = puzzleFigure.View;
            }

            var events = systems.GetShared<SystemsSharedData>().EventsBus;
            //events.NewEventSingleton<CheckOnEndGameComponent>();
            //events.NewEventSingleton<DecreaseAllFiguresScaleComponent>();
            events.NewEventSingleton<FiguresWereSpawnedEvent>();
            events.NewEventSingleton<FindPlaceForFiguresEvent>();
            
            ref var e = ref events.NewEventSingleton<AddPowerUpEvent>();
            e.Data = new bool[_spawnPoints.Length];
        }
        
        
        private int GetNewSpawnFigureIndex()
        {
            var weight = Random.Range(float.Epsilon, _allWeight - float.Epsilon);

            for (var i = 1; i < _figureWeights.Length; i++)
            {
                if (weight > _figureWeights[i - 1] && weight <= _figureWeights[i])
                {
                    return i - 1;
                }
            }

            throw new Exception("Incorrect weight array or weight generation");
        }
    }
}