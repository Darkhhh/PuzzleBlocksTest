using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.Mono;
using Source.Code.SharedData;
using Source.Code.Views;
using UnityEngine;

namespace Source.Code.PreGameplayRunSystems
{
    /// <summary>
    /// Добавляет усиления на фигуры, согласно данным, определенным в AssignPowerUpToFigureSystem
    /// </summary>
    public class AddPowerUpOnFigureSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region ECS Filters

        private readonly EcsFilterInject<Inc<PuzzleFigureComponent>> _puzzleFiguresFilter = default;

        #endregion
        
        
        #region ECS Pools

        private readonly EcsPoolInject<PuzzleFigureComponent> _puzzleFigureComponents = default;
        private readonly EcsPoolInject<FigurePowerUpComponent> _figurePowerUpComponents = default;

        #endregion


        private readonly PowerUpsHandler _handler;
        private EventsBus _events;
        
        public AddPowerUpOnFigureSystem(PowerUpsHandler handler)
        {
            _handler = handler;
            _handler.Init();
        }
        
        
        public void Init(IEcsSystems systems) => _events = systems.GetShared<SystemsSharedData>().EventsBus;
        
        
        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<AddPowerUpEvent>()) return;
            ref var data = ref _events.GetEventBodySingleton<AddPowerUpEvent>();

            for (var i = 0; i < _puzzleFiguresFilter.Value.GetEntitiesCount(); i++)
            {
                if (!data.Data[i]) continue;
            
                var figureEntity = _puzzleFiguresFilter.Value.GetRawEntities()[i];
                ref var figure = ref _puzzleFigureComponents.Value.Get(figureEntity);
                ref var figurePowerUp = ref _figurePowerUpComponents.Value.Add(figureEntity);
            
                var powerUp = SelectPowerUp();
                figurePowerUp.Type = powerUp;
                figurePowerUp.BlockNumber = SetPowerUp(figure.View, powerUp, out var powerUpView);
                figurePowerUp.View = powerUpView;
            }
            
            _events.DestroyEventSingleton<AddPowerUpEvent>();
        }


        private PowerUpType SelectPowerUp() => _handler.GetRandomPowerUpType();

        private int SetPowerUp(PuzzleFigureView view, PowerUpType type, out PowerUpView powerUp)
        {
            var blockNumber = -1;
            var blockPosition = Vector3.zero;
            var blocksPositions = view.GetRelativeBlockPositions();
            if (blocksPositions.Length > 0)
            {
                blockNumber = Random.Range(0, blocksPositions.Length);
                blockPosition = blocksPositions[blockNumber];
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