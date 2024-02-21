using UnityEngine;

public class LevelUp : PowerUp
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        collision.gameObject.GetComponent<Player>().UpgradeTank();
        Destroy(gameObject);
    }
}
