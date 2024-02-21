using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    [SerializeField] private GameObject _firebaseDBManager;
    [SerializeField] private GameObject _audioManager;

    public override void InstallBindings()
    {
        InstallSoundManager();
        InstallFirebaseDbManager();
    }

    private void InstallSoundManager()
    {
        Container
          .Bind<AudioManager>()
          .FromInstance(_audioManager.GetComponent<AudioManager>())
          .AsSingle()
          .NonLazy();
    }
    private void InstallFirebaseDbManager()
    {
        Container
          .Bind<FireBaseManager>()
          .FromInstance(_firebaseDBManager.GetComponent<FireBaseManager>())
          .AsSingle()
          .NonLazy();
    }

}
