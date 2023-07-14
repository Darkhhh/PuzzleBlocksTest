using Source.Data;
using UnityEngine;
using Zenject;

namespace Source.Code.Common.MonoInstallers
{
    public class DataManagerInstaller : MonoInstaller
    {
        [SerializeField] private DataManager handler;
    
        public override void InstallBindings()
        {
            Container
                .Bind<IDataHandler>()
                .To<DataManager>()
                .FromInstance(handler)
                .AsSingle();
            Container.QueueForInject(handler);
        }
    }
}