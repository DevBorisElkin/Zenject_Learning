using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LocationInstaller : MonoInstaller
{
    public Transform PlayerInitialPos;
    public PlayerController PlayerPrefab;
    public InputService InputServicePrefab;
    public CinemachineVirtualCamera mainCamera;

    public override void InstallBindings()
    {
        Debug.Log("InstallBindings() for LocationInstaller");

        BindInputService();
        BindPlayer();
    }

    void BindInputService()
    {
    // Создали GameObject, затем зарезолвили зависимости, от которых данный тип зависит (прописан Constructor / [Inject] в создаваемом типе)
    // и если зависимости от которых он зависит были добавлены в пул Зенджекта, иначе получим исключение 
        var inputService = Container
            .InstantiatePrefabForComponent<InputService>(InputServicePrefab, Vector3.zero, Quaternion.identity, null);

        // Добавили зависимость в пул Зенджекта, нужно делать если другие объекты будут использовать этот тип как зависимость
        // .To<InputService>() Используем только т.к. типы, зависимы от IInput, а не от класса (дали знать зенджекту что
        // IInput берем из нашего инстанса с типом InputService) .FromInstance(inputService)
        Container.Bind<IInput>()
        .To<InputService>()
        .FromInstance(inputService)
        .AsSingle()
        .NonLazy();
    }

    void BindPlayer()
    {
        var heroController = Container
            .InstantiatePrefabForComponent<PlayerController>(PlayerPrefab, PlayerInitialPos.position, Quaternion.identity, null);

        mainCamera.Follow = heroController.transform;
        mainCamera.LookAt = heroController.transform;
    }
}
