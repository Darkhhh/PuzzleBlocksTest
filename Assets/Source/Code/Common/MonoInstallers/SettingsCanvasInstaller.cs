using Source.UI.Code.Pages;
using UnityEngine;
using Zenject;

namespace Source.Code.Common.MonoInstallers
{
    public class SettingsCanvasInstaller : MonoInstaller
    {
        [SerializeField] private SettingsUIHandler handler;
        public override void InstallBindings()
        {
            Container
                .Bind<SettingsUIHandler>()
                .FromInstance(handler)
                .AsSingle();
            Container.QueueForInject(handler);
        }
    }
}