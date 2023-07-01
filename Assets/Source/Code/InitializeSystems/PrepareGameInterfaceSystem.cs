using Leopotam.EcsLite;
using UI.InGame;

namespace Source.Code.InitializeSystems
{
    public class PrepareGameInterfaceSystem : IEcsInitSystem
    {
        private readonly InGameUserInterfaceHandler _handler;

        public PrepareGameInterfaceSystem(InGameUserInterfaceHandler handler)
        {
            _handler = handler;
        }

        public void Init(IEcsSystems systems)
        {
            //_handler.Initialize();
        }
    }
}