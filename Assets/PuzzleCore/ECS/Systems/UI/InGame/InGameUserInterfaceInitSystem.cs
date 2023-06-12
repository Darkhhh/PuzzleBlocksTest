using Leopotam.EcsLite;
using UI.InGame;

namespace PuzzleCore.ECS.Systems.UI.InGame
{
    public class InGameUserInterfaceInitSystem : IEcsInitSystem
    {
        private readonly InGameUserInterfaceHandler _handler;

        public InGameUserInterfaceInitSystem(InGameUserInterfaceHandler handler)
        {
            _handler = handler;
        }

        public void Init(IEcsSystems systems)
        {
            _handler.Initialize();
        }
    }
}