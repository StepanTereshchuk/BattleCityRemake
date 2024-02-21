using UnityEngine;

public class Shovel : PowerUp
{
    private GridMap _gridMap;

    protected override void Start()
    {
        base.Start();
        _gridMap = GameObject.FindGameObjectWithTag("GridMap").GetComponent<GridMap>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        _gridMap.ActivateSpadePower();
        Destroy(gameObject);
    }
}
