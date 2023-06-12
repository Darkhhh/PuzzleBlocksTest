using System.Linq;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Views;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.FigureHandling
{
    public class AddPowerUpOnFigureSystem : IEcsRunSystem
    {
        #region ECS Filters

        private readonly EcsFilterInject<Inc<PuzzleFigureComponent>> _puzzleFiguresFilter = default;
        private readonly EcsFilterInject<Inc<PowerUpToFigureComponent>> _powerUpToFigureFilter = default;

        #endregion
        
        
        #region ECS Pools

        private readonly EcsPoolInject<PowerUpToFigureComponent> _powerUpToFigureComponents = default;
        private readonly EcsPoolInject<PuzzleFigureComponent> _puzzleFigureComponents = default;
        private readonly EcsPoolInject<FigurePowerUpComponent> _figurePowerUpComponents = default;

        #endregion


        private readonly PowerUpsHandler _handler;
        
        public AddPowerUpOnFigureSystem(PowerUpsHandler handler)
        {
            _handler = handler;
            _handler.Init();
        }
        
        
        public void Run(IEcsSystems systems)
        {
            var e = _powerUpToFigureFilter.Value.GetRawEntities()[0];
            ref var array = ref _powerUpToFigureComponents.Value.Get(e);

            if (!array.CreateNew) return;
            
            if (array.PowerUpToFigure.All(val => val != true)) return;

            for (var i = 0; i < _puzzleFiguresFilter.Value.GetEntitiesCount(); i++)
            {
                if (!array.PowerUpToFigure[i]) continue;

                var figureEntity = _puzzleFiguresFilter.Value.GetRawEntities()[i];
                ref var figure = ref _puzzleFigureComponents.Value.Get(figureEntity);
                ref var figurePowerUp = ref _figurePowerUpComponents.Value.Add(figureEntity);

                var powerUp = SelectPowerUp();
                figurePowerUp.Type = powerUp;
                figurePowerUp.BlockNumber = SetPowerUp(figure.View, powerUp, out var powerUpView);
                figurePowerUp.View = powerUpView;
                array.PowerUpToFigure[i] = false;
            }

            array.CreateNew = false;
        }


        private PowerUpType SelectPowerUp()
        {
            return _handler.GetRandomPowerUpType();
        }

        private int SetPowerUp(PuzzleFigureView view, PowerUpType type, out PowerUpView powerUp)
        {
            var blockNumber = -1;
            var blockPosition = Vector3.zero;
            if (view.BlocksRelativePositions.Length > 0)
            {
                blockNumber = Random.Range(0, view.BlocksRelativePositions.Length);
                blockPosition = view.BlocksScenePosition[blockNumber];
            }

            powerUp = _handler.GetPowerUp(type);
            var powerUpTransform = powerUp.transform;
            var viewTransform = view.transform;
            powerUpTransform.position = blockPosition + viewTransform.position;
            powerUpTransform.parent = viewTransform;
            
            return blockNumber;
        }
    }
}