using Leopotam.EcsLite;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;
using Temp.Entrance.Systems;
using Temp.SharedData;
using UnityEngine;

namespace Temp.Entrance
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
                .Add(new EcsEditorSystems())
                .Add(new InitializeSceneSystems())
                .Add(new PreGameplaySystems())
                //.Add(new GameplaySystems())
                //.Add(new PostGameplaySystems())
                //.Add(new CleanUpSystems())
                //.Add(new UpdateUserInterfaceSystems())
                .Init();
        }

        private void Update()
        {
            _container.Run();
        }

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