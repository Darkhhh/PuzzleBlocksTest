using Source.UI.Code.Pages;
using UnityEngine;
using Zenject;

namespace Source.Code.Common.MonoInstallers
{
    public class PauseCanvasInstaller : MonoInstaller
    {
        [SerializeField] private PauseUIHandler handler;
        public override void InstallBindings()
        {
            Container
                .Bind<PauseUIHandler>()
                .FromInstance(handler)
                .AsSingle();
            Container.QueueForInject(handler);
        }
    }
}