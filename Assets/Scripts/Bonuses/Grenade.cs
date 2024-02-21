using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Grenade : PowerUp
{
    private GamePlayManager _gamePlayManager;
    private int _grenadeDamage = 1000;
  
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
        List<EnemyAI> enemies = _gamePlayManager.enemyStash;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Health>().TakeDamage(_grenadeDamage, true);
        }
        Destroy(gameObject);
    }
}
