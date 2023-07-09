using Source.Localization;
using UnityEngine;
using Zenject;

namespace Source.Code.Common.MonoInstallers
{
    public class LocalizationHandlerInstaller : MonoInstaller
    {
        [SerializeField] private LocalizationHandler handler;
    
        public override void InstallBindings()
        {
            handler.Init();
            Container
                .Bind<ILocalizationHandler>()
                .To<LocalizationHandler>()
                .FromInstance(handler)
                .AsSingle();
            Container.QueueForInject(handler);
        }
    }
}