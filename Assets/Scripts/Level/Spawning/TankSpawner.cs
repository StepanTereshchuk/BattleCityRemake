using UnityEngine;
using Zenject;

public abstract class TankSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject smallTank;
    [SerializeField] protected GameObject fastTank;
    [SerializeField] protected GameObject bigTank;
    [SerializeField] protected GameObject armoredTank;
    protected GameObject[] tanks;
    protected GameObject tank;
    protected GamePlayManager gamePlayManager;
    protected LevelManager levelManager;
    protected UIManager uiManager;

    protected enum tankType
    {
        smallTank,
        fastTank,
        bigTank,
        armoredTank
    };

    [Inject] protected DiContainer _container;

    [Inject]
    private void Construct(GamePlayManager gamePlayManager, LevelManager levelManager, UIManager uiManager)
    {
        this.gamePlayManager = gamePlayManager;
        this.levelManager = levelManager;
        this.uiManager = uiManager;
    }

    public virtual void ActivateSpawnedTank()
    {
        if (tank != null)
        {
            tank.SetActive(true);
        }
    }

    public virtual void StartSpawning()
    {

    }

    public virtual void IncreaseTankAmount()
    {

    }
    protected virtual void Initialize()
    {

    }
    
    private void Start()
    {
        Initialize();
    }
}