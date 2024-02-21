using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Projectile : MonoBehaviour
{
    public float damage { get; private set; }
    protected Tilemap tilemap;
    protected Rigidbody2D rigidBody;
    protected float speed = 1;
    protected bool toBeDestroyed = false;
    protected bool destroySteel = false;

    public void DestroyProjectile()
    {
        if (gameObject.activeSelf == false)
        {
            Destroy(gameObject);
        }
        toBeDestroyed = true;
    }

    public void InitializeWithData(float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;
    }

    public void IncreaseDamage()
    {
        destroySteel = true;
    }
    public void IncreaseSpeed(float speed)
    {
        this.speed = speed;
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        damage = 1;
        rigidBody.velocity = transform.up * speed;
    }

    protected virtual void OnEnable()
    {
        if (rigidBody != null)
        {
            rigidBody.velocity = transform.up * speed;
        }
    }

    private void OnDisable()
    {
        if (toBeDestroyed)
        {
            Destroy(this.gameObject);
        }
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision) // for tilemap
    {
        rigidBody.velocity = Vector2.zero;
        tilemap = collision.gameObject.GetComponent<Tilemap>();

        if (collision.gameObject.CompareTag("Brick") || (destroySteel && collision.gameObject.CompareTag("Steel")))
        {        
            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
            }
        }
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) // for tanks
    {
        if (collision.gameObject.GetComponent<Health>() != null)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage();
        }
        gameObject.SetActive(false);
    }
}