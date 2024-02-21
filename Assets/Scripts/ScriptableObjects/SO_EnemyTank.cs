using UnityEngine;
[CreateAssetMenu(fileName = "EnemyTankData", menuName = "ScriptableObjects/Tanks/EnemyTank", order = 1)]
public class SO_EnemyTank : SO_GenericTank
{
    [SerializeField] private float blinkRepeatRate;
    [SerializeField] private float blinkDelay;
    [SerializeField] protected float minTimeBeforeFire = 3f;
    [SerializeField] protected float maxTimeBeforeFire = 5f;
    [SerializeField] protected float minTimeBeforeChangeDirection = 2f;
    [SerializeField] protected float maxTimeBeforeChangeDirection = 6f;
    public float BlinkRepeatRate => blinkRepeatRate;
    public float BlinkDelay => blinkDelay;
    public float MinTimeBeforeFire => minTimeBeforeFire;
    public float MaxTimeBeforeFire => maxTimeBeforeFire;
    public float MinTimeBeforeChangeDirection => minTimeBeforeChangeDirection;
    public float MaxTimeBeforeChangeDirection => maxTimeBeforeChangeDirection;
}