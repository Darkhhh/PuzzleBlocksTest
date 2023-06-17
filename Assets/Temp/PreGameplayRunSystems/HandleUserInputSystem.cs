﻿using Leopotam.EcsLite;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace Temp.PreGameplayRunSystems
{
    public class HandleUserInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly Camera _sceneCamera;
        private EventsBus _events;
        private InGameData _gameData;
        
        public HandleUserInputSystem(Camera sceneCamera)
        {
            _sceneCamera = sceneCamera;
        }
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
            _gameData = systems.GetShared<SystemsSharedData>().GameData;
        }
        
        public void Run(IEcsSystems systems)
        {
            var mousePosition = _sceneCamera.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                ref var e = ref _events.NewEventSingleton<LeftMouseDownEvent>();
                e.Position = mousePosition;
            }
            if (Input.GetMouseButtonDown(1))
            {
                ref var e = ref _events.NewEventSingleton<RightMouseDownEvent>();
                e.Position = mousePosition;
            }
            
            _gameData.CurrentMousePosition = mousePosition;
        }
    }
}