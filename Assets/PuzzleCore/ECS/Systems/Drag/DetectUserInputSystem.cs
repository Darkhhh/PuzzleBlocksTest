using Leopotam.EcsLite;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.Drag
{
    /// <summary>
    /// Создает событие левого и правого клика мышкой, а также событие текущей позиции мыши
    /// </summary>
    public class DetectUserInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly Camera _sceneCamera;
        private EventsBus _events;
        
        public DetectUserInputSystem(Camera sceneCamera)
        {
            _sceneCamera = sceneCamera;
        }
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }
        
        public void Run(IEcsSystems systems)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ref var e = ref _events.NewEventSingleton<LeftMouseDownEvent>();
                e.Position = _sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.GetMouseButtonDown(1))
            {
                ref var e = ref _events.NewEventSingleton<RightMouseDownEvent>();
                e.Position = _sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            }

            //TODO Не создавать событие, а обновлять данные в GameData классе
            ref var mousePositionEvent = ref _events.NewEventSingleton<CurrentMousePositionEvent>();
            mousePositionEvent.Position = _sceneCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}