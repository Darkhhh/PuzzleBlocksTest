using Source.UI.Code.Pages;
using UnityEngine;
using Zenject;

namespace Source.Code.Common.MonoInstallers
{
    public class ModalWindowInstaller : MonoInstaller
    {
        [SerializeField] private EndGameModalHandler handler;
        public override void InstallBindings()
        {
            Container
                .Bind<EndGameModalHandler>()
                .FromInstance(handler)
                .AsSingle();
            Container.QueueForInject(handler);
        }
    }
}