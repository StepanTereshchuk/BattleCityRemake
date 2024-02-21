using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private GameObject _gamePlayManagerPrefab;
    [SerializeField] private GameObject _levelManagerPrefab;
    [SerializeField] private GameObject _uiManagerPrefab;
    [SerializeField] private GameObject _audioManager;
    [SerializeField] private GameObject _scalerManager;

    public override void InstallBindings()
    {
        InstalLevelManager();
        InstalGamePlayManager();
        InstalGameUiManager();
        InstallSoundManager();
        InstallScalerManager();
    }

    private void InstallScalerManager()
    {
        Container
         .Bind<GameScaler>()
         .FromInstance(_scalerManager.GetComponent<GameScaler>())
         .AsSingle()
         .NonLazy();
    }

    private void InstallSoundManager()
    {
        Container
          .Bind<AudioManager>()
          .FromInstance(_audioManager.GetComponent<AudioManager>())
          .AsSingle()
          .NonLazy();
    }

    private void InstalLevelManager()
    {
        Container
           .Bind<LevelManager>()
           .FromInstance(_levelManagerPrefab.GetComponent<LevelManager>())
           .AsSingle()
           .NonLazy();
    }

    private void InstalGameUiManager()
    {
        Container
           .Bind<UIManager>()
           .FromInstance(_uiManagerPrefab.GetComponent<UIManager>())
           .AsSingle()
           .NonLazy();
    }

    private void InstalGamePlayManager()
    {
        Container
           .Bind<GamePlayManager>()
           .FromInstance(_gamePlayManagerPrefab.GetComponent<GamePlayManager>())
           .AsSingle()
           .NonLazy();
    }
}