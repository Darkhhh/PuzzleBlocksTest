using Source.UI.Code.Menu.Pages;
using UnityEngine;
using Zenject;

namespace Source.Code.Common.MonoInstallers
{
    public class MenuCanvasInstaller : MonoInstaller
    {
        [SerializeField] private MenuUIHandler handler;
        public override void InstallBindings()
        {
            Container
                .Bind<MenuUIHandler>()
                .FromInstance(handler)
                .AsSingle();
            Container.QueueForInject(handler);
        }
    }
}