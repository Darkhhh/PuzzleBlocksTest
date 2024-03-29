using Source.UI.Code.Menu.Pages;
using UnityEngine;
using Zenject;

namespace Source.Code.Common.MonoInstallers
{
    public class MarketCanvasInstaller : MonoInstaller
    {
        [SerializeField] private MarketUIHandler handler;
        public override void InstallBindings()
        {
            Container
                .Bind<MarketUIHandler>()
                .FromInstance(handler)
                .AsSingle();
            Container.QueueForInject(handler);
        }
    }
}