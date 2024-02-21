using UnityEngine;

public class SO_GenericTank : ScriptableObject
{
    [SerializeField] protected int actualHealth;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected LayerMask blockingLayer;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float projectileDamage;
    [SerializeField] protected GameObject projectilePrefab;
    protected int _invincibleHealthAmount = 1000;

    public float Speed => moveSpeed;
    public int Health => actualHealth;
    public LayerMask BlockingLayerMask => blockingLayer;
    public float ProjectileSpeed => projectileSpeed;
    public float ProjectileDamage => projectileDamage;
    public GameObject ProjectilePrefab => projectilePrefab;
    public int InvincibleHpAmount => _invincibleHealthAmount;

    // fire effects ???
    // audio
}
