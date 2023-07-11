using Source.Code.Common.Audio;
using UnityEngine;
using Zenject;

namespace Source.Code.Common.MonoInstallers
{
    public class AudioManagerInstaller : MonoInstaller
    {
        [SerializeField] private GameObject prefab;
        public override void InstallBindings()
        {
            var instance = Container.InstantiatePrefabForComponent<AudioManager>(prefab);
            Container
                .Bind<AudioManager>()
                .FromInstance(instance)
                .AsSingle();
            Container.QueueForInject(instance);
        }
    }
}