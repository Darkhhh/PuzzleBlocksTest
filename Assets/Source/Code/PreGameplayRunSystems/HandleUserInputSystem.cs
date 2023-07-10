using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components.Events;
using Source.Code.SharedData;
using UnityEngine;

namespace Source.Code.PreGameplayRunSystems
{
    public class HandleUserInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly Camera _sceneCamera;
        private EventsBus _events;
        private InGameData _gameData;
        
        public HandleUserInputSystem(Camera sceneCamera) => _sceneCamera = sceneCamera;
        
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
            if (Input.GetMouseButtonUp(0))
            {
                ref var e = ref _events.NewEventSingleton<RightMouseDownEvent>();
                e.Position = mousePosition;
            }
            
            _gameData.CurrentMousePosition = mousePosition;
        }
    }
}