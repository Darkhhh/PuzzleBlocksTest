using System;
using System.Linq;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using Random = UnityEngine.Random;

namespace PuzzleCore.ECS.Systems.FigureHandling
{
    public class AssignPowerUpToFigureSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region ECS Filters

        private readonly EcsFilterInject<Inc<PuzzleFigureComponent>> _puzzleFiguresFilter = default;
        private readonly EcsFilterInject<Inc<PowerUpToFigureComponent>> _powerUpToFigureFilter = default;

        #endregion
        
        
        #region ECS Pools

        private readonly EcsPoolInject<PowerUpToFigureComponent> _powerUpToFigureComponents = default;

        #endregion

        
        #region Private Values

        private readonly int _numberOfFigures;
        
        private readonly float _probability;

        #endregion
        
        
        
        public AssignPowerUpToFigureSystem(int numberOfFigures, float probability)
        {
            _numberOfFigures = numberOfFigures;
            _probability = probability;
        }
        
        
        public void Init(IEcsSystems systems)
        {
            var entity = systems.GetWorld().NewEntity();

            ref var array = ref _powerUpToFigureComponents.Value.Add(entity);

            array.PowerUpToFigure = new bool[_numberOfFigures];
        }

        
        public void Run(IEcsSystems systems)
        {
            if (_puzzleFiguresFilter.Value.GetEntitiesCount() != 0) return;

#if UNITY_EDITOR
            if (_powerUpToFigureFilter.Value.GetEntitiesCount() > 1)
                throw new Exception("Not a single power ups array");
#endif

            foreach (var entity in _powerUpToFigureFilter.Value)
            {
                ref var array = ref _powerUpToFigureComponents.Value.Get(entity);

                for (var i = 0; i < array.PowerUpToFigure.Length; i++)
                {
                    array.PowerUpToFigure[i] = Random.Range(0, 1) < _probability;
                }

                array.CreateNew = array.PowerUpToFigure.Any(val => val != false);
            }
        }
    }
}