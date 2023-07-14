using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Common.Audio;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.SharedData;
using Source.Code.Views.ManualPowerUp;
using Source.Data;

namespace Source.Code.GameplaySystems
{
    public class ReleaseManualPowerUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ManualPowerUp, ReleasedObjectComponent>> _releasedManualPowerUpFilter = default;
        private readonly EcsFilterInject<Inc<TargetedCellStateComponent>> _targetedCellsFilter = default;
        
        private EventsBus _events;
        private GameData _data;
        private AudioManager _audio;


        public void Init(IEcsSystems systems)
        {
            var shared = systems.GetShared<SystemsSharedData>();
            _events = shared.EventsBus;
            _data = shared.SceneData.DataManager.GetData().GameData;
            _audio = shared.SceneData.audioManager;
        }
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (var powerUpEntity in _releasedManualPowerUpFilter.Value)
            {
                ref var dragData = ref _releasedManualPowerUpFilter.Pools.Inc2.Get(powerUpEntity);
                ref var manualPowerUp = ref _releasedManualPowerUpFilter.Pools.Inc1.Get(powerUpEntity);
                
                manualPowerUp.View.transform.position = dragData.InitialPosition;

                if (_targetedCellsFilter.Value.GetEntitiesCount() > 0)
                {
                    manualPowerUp.AvailableAmount--;

                    switch (manualPowerUp.View.Type)
                    {
                        case ManualPowerUpType.CanonBall:
                            _audio.Play(SoundTag.ActivatingDynamite);
                            _data.canonBallAmount = manualPowerUp.AvailableAmount;
                            break;
                        case ManualPowerUpType.Broomstick:
                            _audio.Play(SoundTag.ActivatingBroomstick);
                            _data.broomstickAmount = manualPowerUp.AvailableAmount;
                            break;
                        case ManualPowerUpType.Dynamite:
                            _audio.Play(SoundTag.ActivatingDynamite);
                            _data.dynamiteAmount = manualPowerUp.AvailableAmount;
                            break;
                        case ManualPowerUpType.LargeDynamite:
                            _audio.Play(SoundTag.ActivatingDynamite);
                            _data.largeDynamiteAmount = manualPowerUp.AvailableAmount;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    manualPowerUp.View.SetAmountText(manualPowerUp.AvailableAmount);
                    _events.NewEventSingleton<ClearTargetedCellsEvent>();
                }
                _releasedManualPowerUpFilter.Pools.Inc2.Del(powerUpEntity);
            }
        }
    }
}