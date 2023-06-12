using Leopotam.EcsLite;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;
using UI.InGame;

namespace PuzzleCore.ECS.Systems.UI.InGame
{
    public class UpdateScoreSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly InGameUserInterfaceHandler _handler;
        private EventsBus _events;

        public UpdateScoreSystem(InGameUserInterfaceHandler handler)
        {
            _handler = handler;
        }


        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }

        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<UpdateScoreEvent>()) return;
            
            ref var data = ref _events.GetEventBodySingleton<UpdateScoreEvent>();
            _handler.SetScore(data.NewScore);
        }
    }
}