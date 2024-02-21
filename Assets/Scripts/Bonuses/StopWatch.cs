using UnityEngine;
using Zenject;

public class StopWatch : PowerUp
{
    private GamePlayManager _gamePlayManager;

    [Inject]
    private void Construct(GamePlayManager gamePlayManager)
    {
        _gamePlayManager = gamePlayManager;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        _gamePlayManager.ActivateFreeze();
        Destroy(this.gameObject);
    }
}
