using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace PuzzleCore.ECS
{
    public class EditorEcsStartUp : MonoBehaviour
    {
#if UNITY_EDITOR
        private IEcsSystems _editorSystems;
#endif
        private bool _initialized = false;

        public void Initialize(EcsWorld world)
        {
#if UNITY_EDITOR
            // Создаем отдельную группу для отладочных систем.
            _editorSystems = new EcsSystems (world);
            _editorSystems
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Init();
            
            _initialized = true;
#endif
        }

        private void Update()
        {
            if (!_initialized) return;
#if UNITY_EDITOR
            // Выполняем обновление состояния отладочных систем. 
            _editorSystems?.Run();
#endif
        }

        private void OnDestroy()
        {
            if (!_initialized) return;
#if UNITY_EDITOR
            // Выполняем очистку отладочных систем.
            if (_editorSystems != null) 
            {
                _editorSystems.Destroy();
                _editorSystems = null;
            }
#endif
        }
    }
}
