﻿using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;

namespace Temp.DragSystems
{
    public class ReleaseDraggingObjectSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DraggingObjectComponent>> _draggingObjectFilter = default;
        
        private readonly EcsPoolInject<DraggingObjectComponent> _draggingObjectComponents = default;
        private readonly EcsPoolInject<ReleasedObjectComponent> _releasedObjectComponents = default;
        
        private EventsBus _events;
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }

        public void Run(IEcsSystems systems)
        {
            if (_draggingObjectFilter.Value.GetEntitiesCount() == 0) return;
            if (!_events.HasEventSingleton<RightMouseDownEvent>()) return;

            foreach (var entity in _draggingObjectFilter.Value)
            {
                ref var dragComponent = ref _draggingObjectComponents.Value.Get(entity);
                ref var releaseComponent = ref _releasedObjectComponents.Value.Add(entity);

                releaseComponent.InitialPosition = dragComponent.InitialPosition;
                _draggingObjectComponents.Value.Del(entity);
            }
        }
    }
}