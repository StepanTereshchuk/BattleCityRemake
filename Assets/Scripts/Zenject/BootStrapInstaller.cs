using UnityEngine;
using Zenject;

public class BootStrapInstaller : MonoInstaller
{
    public GameObject sceneTrackerPrefab;
    public GameObject firebaseManagerPrefab;
    public override void InstallBindings()
    {
        InstallSceneTracker();
        InstallFirebaseDatabase();
    }

    private void InstallFirebaseDatabase()
    {
        FireBaseManager fireBaseManager = Container
            .InstantiatePrefabForComponent<FireBaseManager>(firebaseManagerPrefab, Vector3.zero, Quaternion.identity, null);

        Container
            .Bind<FireBaseManager>()
            .FromInstance(fireBaseManager)
            .AsSingle()
            .NonLazy();
    }

    private void InstallSceneTracker()
    {
        SceneTracker sceneTracker = Container
           .InstantiatePrefabForComponent<SceneTracker>(sceneTrackerPrefab, Vector3.zero, Quaternion.identity, null);

        Container
            .Bind<SceneTracker>()
            .FromInstance(sceneTracker)
            .AsSingle()
            .NonLazy();
    }
}