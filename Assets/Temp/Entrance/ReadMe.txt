Общая архитектура происходящего следующая:

1.  Скрипт EntryPoint наследник MonoBehaviour находится на сцене. 
    Также на сцене находится скрипт Temp/SharedData/SceneData.cs, который передан по прямой ссылке в EntryPoint.
2.  Все ECS-системы объединяются по смыслу (инициализирующие, debug, UI)  в классы, реализующие IEntryEcsSystems.
3.  Затем все IEntryEcsSystems в скрипте EntryPoint добавляются в SystemsContainer по очередности выполнения.

То есть все выглядит так:

IEcsRunSystem or/and IEcsInitSystem --> IEntrySystems --> SystemsContainer --> EntryPoint