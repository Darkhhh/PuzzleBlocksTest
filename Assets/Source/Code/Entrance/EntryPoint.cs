using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using Source.Code.Entrance.EntrySystems;
using Source.Code.SharedData;
using Source.Localization;
using Source.UI.Code;
using Source.UI.Code.InGamePageManagerScripts;
using UnityEngine;
using Zenject;

namespace Source.Code.Entrance
{
    public class EntryPoint : MonoBehaviour
    {
        #region Private

        private SystemsContainer _container;
        private EcsWorld _world;
        private SystemsSharedData _sharedData;

        #endregion


        #region Serialized

        [SerializeField] private SceneData sceneData;

        #endregion
        

        private void Start()
        {
            _world = new EcsWorld();
            _sharedData = new SystemsSharedData
            {
                EventsBus = new EventsBus(), 
                GameData = new InGameData(), 
                SceneData = sceneData
            };

            _container = new SystemsContainer(_world, _sharedData);
            _container
                .Add(new EcsEditorEntryEcsSystems())
                .Add(new InitializeSceneEntryEcsSystems())
                .Add(new PreGameplayEntryEcsSystems())
                .Add(new GameplayEntryEcsSystems())
                .Add(new AnimationsEntryEcsSystems())
                .Add(new PostGameplayEntryEcsSystems())
                .Add(new CleanUpEntryEcsSystems())
                .Add(new UpdateUserInterfaceEntryEcsSystems())
                .Init();
            
            sceneData.pageManager.Init(_sharedData.EventsBus, sceneData.localizationHandler, Language.Russian);
        }

        
        private void Update() => _container.Run();

        
        private void OnDestroy()
        {
            if (_container == null) return;
            
            _sharedData.EventsBus.Destroy();
            _container.Destroy();
            _container = null;
            _world.Destroy();
        }
    }
}