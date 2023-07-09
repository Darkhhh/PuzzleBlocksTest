using Source.UI.Code.Pages;
using UnityEngine;
using Zenject;

namespace Source.Code.Common.MonoInstallers
{
    public class GameCanvasInstaller : MonoInstaller
    {
        [SerializeField] private InGameUIHandler handler;
        public override void InstallBindings()
        {
            Container
                .Bind<InGameUIHandler>()
                .FromInstance(handler)
                .AsSingle();
            Container.QueueForInject(handler);
        }
    }
}