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
    // ������� GameObject, ����� ����������� �����������, �� ������� ������ ��� ������� (�������� Constructor / [Inject] � ����������� ����)
    // � ���� ����������� �� ������� �� ������� ���� ��������� � ��� ���������, ����� ������� ���������� 
        var inputService = Container
            .InstantiatePrefabForComponent<InputService>(InputServicePrefab, Vector3.zero, Quaternion.identity, null);

        // �������� ����������� � ��� ���������, ����� ������ ���� ������ ������� ����� ������������ ���� ��� ��� �����������
        // .To<InputService>() ���������� ������ �.�. ����, �������� �� IInput, � �� �� ������ (���� ����� ��������� ���
        // IInput ����� �� ������ �������� � ����� InputService) .FromInstance(inputService)
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
