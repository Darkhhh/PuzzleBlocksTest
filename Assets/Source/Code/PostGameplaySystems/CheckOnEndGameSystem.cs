﻿using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components;
using Source.Code.SharedData;
using UnityEngine;

namespace Source.Code.PostGameplaySystems
{
    public class CheckOnEndGameSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent>> _figuresFilter = default;
        private readonly EcsFilterInject<Inc<CanNotBeTakenComponent>> _notTakenFiguresFilter = default;

        private EventsBus _events;
        
        public void Init(IEcsSystems systems) => _events = systems.GetShared<SystemsSharedData>().EventsBus;

        public void Run(IEcsSystems systems)
        {
            if (_figuresFilter.Value.GetEntitiesCount() == _notTakenFiguresFilter.Value.GetEntitiesCount())
                Debug.Log("Game Over!");
        }
    }
}