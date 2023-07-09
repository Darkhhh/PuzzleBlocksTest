using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components.Events;
using Source.Code.SharedData;
using Source.UI.InGame;

namespace Source.Code.UpdateUserInterfaceSystems
{
    public class UpdateInGameUISystem : IEcsInitSystem, IEcsRunSystem
    {
        private EventsBus _events;

        private readonly IGameUIHandler _handler;

        public UpdateInGameUISystem(IGameUIHandler handler) => _handler = handler;
        
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
            _handler.Init(_events);
        }

        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<UpdateInGameUIEvent>()) return;

            ref var data = ref _events.GetEventBodySingleton<UpdateInGameUIEvent>();
            
            _handler.SetNewScore(data.NewScore);
            
            _handler.SetNewCoinsAmount(data.NewCoins);
            
            _events.DestroyEventSingleton<UpdateInGameUIEvent>();
        }
    }
}