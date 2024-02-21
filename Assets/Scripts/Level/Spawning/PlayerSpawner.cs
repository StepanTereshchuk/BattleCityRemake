using UnityEngine;
using Zenject;

public class PlayerSpawner : TankSpawner
{
    private GameScaler _scaler;
    [Inject]
    private void Construct( LevelManager levelManager, GameScaler gameScaler)
    {
        this.levelManager = levelManager;
        _scaler = gameScaler;
    }
    public override void StartSpawning()
    {
       
        tank = _container.InstantiatePrefab(smallTank,transform.position, transform.rotation, null);
        //_scaler.ApplyProperPrefabScaling(tank);
    }

    public override void ActivateSpawnedTank()
    {
        base.ActivateSpawnedTank();
    }

    public override void IncreaseTankAmount()
    {
        base.IncreaseTankAmount();
    }

    protected override void Initialize()
    {
        tanks = new GameObject[1] { smallTank };
    }
}
