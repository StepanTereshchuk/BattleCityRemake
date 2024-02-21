using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int smallTanksInThisLevel;
    [SerializeField] private int fastTanksInThisLevel;
    [SerializeField] private int bigTanksInThisLevel;
    [SerializeField] private int armoredTanksInThisLevel;
    [SerializeField] private int stageNumber;
    [SerializeField] private float spawnRateInThisLevel = 5f;
    [SerializeField] private float bonusCrateRateInThisLevel = 0.2f;

    public int smallTanks;
    public int fastTanks;
    public int bigTanks;
    public int armoredTanks;
    public float spawnRate { get; private set; }
    public float bonusCrateRate { get; private set; }

    private SceneTracker _sceneTracker;

    [Inject]
    private void Construct(SceneTracker sceneTracker)
    {
        _sceneTracker = sceneTracker;
    }

    private void Awake()
    {
        _sceneTracker.stageNumber = stageNumber;
        smallTanks = smallTanksInThisLevel;
        fastTanks = fastTanksInThisLevel;
        bigTanks = bigTanksInThisLevel;
        armoredTanks = armoredTanksInThisLevel;
        spawnRate = spawnRateInThisLevel;
        bonusCrateRate = bonusCrateRateInThisLevel;
    }
}
