using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components.Events;
using Source.Code.SharedData;
using Source.UI.Code.InGamePageManagerScripts;

namespace Source.Code.CleanUpSystems
{
    public class HandleEndGameSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const string ModalPageTag = "xml-endgame";

        private readonly PageManager _pageManager;

        private EventsBus _events;

        public HandleEndGameSystem(PageManager pageManager)
        {
            _pageManager = pageManager;
        }

        public void Init(IEcsSystems systems) => _events = systems.GetShared<SystemsSharedData>().EventsBus;

        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<GameOverEvent>()) return;
            
            _pageManager.OpenPage(ModalPageTag);

            systems.GetShared<SystemsSharedData>().GameData.Pause = true;
        }
    }
}