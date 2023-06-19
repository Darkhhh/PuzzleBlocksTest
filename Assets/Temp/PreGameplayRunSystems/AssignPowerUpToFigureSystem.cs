﻿using Leopotam.EcsLite;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;
using Temp.SharedData;
using Random = UnityEngine.Random;

namespace Temp.PreGameplayRunSystems
{
    /// <summary>
    /// Определяет на каких фигурах будет находиться усиления
    /// </summary>
    public class AssignPowerUpToFigureSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EventsBus _events;
        
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }
        
        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<AddPowerUpEvent>()) return;

            ref var data = ref _events.GetEventBodySingleton<AddPowerUpEvent>();

            for (var i = 0; i < data.Data.Length; i++)
            {
                data.Data[i] = Random.Range(0, 1) < ConstantData.PowerUpProbability;
            }
        }
    }
}