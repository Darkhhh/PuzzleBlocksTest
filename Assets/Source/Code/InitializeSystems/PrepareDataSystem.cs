using Leopotam.EcsLite;
using Source.Code.Components.Events;
using Source.Code.SharedData;

namespace Source.Code.InitializeSystems
{
    public class PrepareDataSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var shared = systems.GetShared<SystemsSharedData>();
            var data = shared.SceneData.DataManager.GetData();

            shared.GameData.CoinsAmount = data.GameData.coinsAmount;

            ref var uiData = ref shared.EventsBus.NewEventSingleton<UpdateInGameUIEvent>();
            uiData.NewScore = 0;
            uiData.NewCoins = shared.GameData.CoinsAmount;
        }
    }
}