using Leopotam.EcsLite;
using Temp.SharedData;

namespace Temp.Entrance.EntrySystems
{
    /// <summary>
    /// Тут находятся отладочные системы для дебага ECS
    /// </summary>
    public class EcsEditorEntryEcsSystems : IEntryEcsSystems
    {
        private bool _initialized = false;

        public EcsSystems Systems { get; set; }
        
        public void Init(EcsWorld world, SystemsSharedData sharedData)
        {
#if UNITY_EDITOR
            // Создаем отдельную группу для отладочных систем.
            Systems = new EcsSystems (world);
            Systems
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Init();
            
            _initialized = true;
#endif
        }

        public void Run()
        {
            if (!_initialized) return;
#if UNITY_EDITOR
            // Выполняем обновление состояния отладочных систем. 
            Systems?.Run();
#endif
        }

        public void Destroy()
        {
            if (!_initialized) return;
#if UNITY_EDITOR
            // Выполняем очистку отладочных систем.
            if (Systems != null) 
            {
                Systems.Destroy();
                Systems = null;
            }
#endif
        }
    }
}