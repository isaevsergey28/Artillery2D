using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private AllParticipants _allParticipants;

    public override void InstallBindings()
    {
        BindCamera();
        BindParticipants();
    }
    private void BindCamera()
    {
        Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();
    }
    private void BindParticipants()
    {
        Container.Bind<AllParticipants>().FromInstance(_allParticipants).AsSingle();
    }
}