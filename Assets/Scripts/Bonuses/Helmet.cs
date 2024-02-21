using UnityEngine;

public class Helmet : PowerUp
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        collision.gameObject.GetComponent<Animator>().SetTrigger("Invincibility");
        Destroy(gameObject);
    }
}
