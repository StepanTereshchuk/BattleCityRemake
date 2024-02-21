using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Zenject;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _bonusCrates;
    [SerializeField] private GameObject _spawnPlayerPoint;
    [SerializeField] private List<EnemySpawner> _spawnEnemyPoints;
    public List<EnemyAI> enemyStash { get; private set; }
    private Tilemap _waterTilemap;
    private Tilemap _steelTilemap;
    private GameScaler _gameScaler;
    private SceneTracker _sceneTracker;
    private LevelManager _levelManager;
    private UIManager _uiManager;
    private AudioManager _audioManager;
    private bool _stageStart = false;
    private bool _tankReserveEmpty = false;
    [Inject] private DiContainer _container;

    [Inject]
    private void Construct(SceneTracker sceneTracker, LevelManager levelManager, UIManager uiManager, AudioManager audioManager, GameScaler gameScaler)
    {
        _sceneTracker = sceneTracker;
        _levelManager = levelManager;
        _audioManager = audioManager;
        _uiManager = uiManager;
        _gameScaler = gameScaler;
    }

    public void ActivateFreeze()
    {
        StartCoroutine(FreezeActivated());
    }

    public void GenerateBonusCrate()
    {
        GameObject bonusCrate = _bonusCrates[Random.Range(0, _bonusCrates.Length)];
        Vector3 cratePosition = _gameScaler.GetRandomPositionInsideScreen(bonusCrate);
        if (InvalidBonusCratePosition(cratePosition))
        {
            do
            {
                cratePosition = _gameScaler.GetRandomPositionInsideScreen(bonusCrate);

                if (!InvalidBonusCratePosition(cratePosition))
                    _container.InstantiatePrefab(bonusCrate, cratePosition, Quaternion.identity, null);
            } while (InvalidBonusCratePosition(cratePosition));
        }
        else
        {
            _container.InstantiatePrefab(bonusCrate, cratePosition, Quaternion.identity, null);
        }
    }

    public void IncreaseEnemyStash(EnemyAI enemy)
    {
        enemyStash.Add(enemy);
    }

    public void DecreaseStash(EnemyAI enemy)
    {
        enemyStash.Remove(enemy);
    }

    public void SpawnPlayer()
    {
        if (_sceneTracker.playerLives > 0)
        {
            if (!_stageStart)
            {
                _sceneTracker.playerLives--;
            }
            _stageStart = false;

            _spawnPlayerPoint.GetComponent<Animator>().SetTrigger("Spawning");
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        StartCoroutine(FinishGame());
    }
    private IEnumerator FinishGame()
    {
        StartCoroutine(_audioManager.PlaySoundAndWaitFinish(SoundKey.Defeat));
        yield return StartCoroutine(_uiManager.ShowGameOverText());
        _sceneTracker.stageCleared = false;
        CalculateStageScore();
    }

    public void SpawnEnemy()
    {
        if (_levelManager.smallTanks + _levelManager.fastTanks + _levelManager.bigTanks + _levelManager.armoredTanks > 0)
        {
            int spawnPointIndex = Random.Range(0, _spawnEnemyPoints.Count - 1);
            _spawnEnemyPoints[spawnPointIndex].GetComponent<Animator>().SetTrigger("Spawning");
        }
        else
        {
            CancelInvoke();
            _tankReserveEmpty = true;
        }
    }

    private void Start()
    {
        enemyStash = new List<EnemyAI>();
        _steelTilemap = GameObject.FindGameObjectWithTag("Steel").GetComponent<Tilemap>();
        _waterTilemap = GameObject.FindGameObjectWithTag("Water").GetComponent<Tilemap>();
        _stageStart = true;

        //foreach (var bonus in _bonusCrates)
        //{
        //    _gameScaler.ApplyProperPrefabScaling(bonus);
        //}

        StartCoroutine(StartSpawningEnemy());
    }

    private void Update()
    {
        if (_tankReserveEmpty && enemyStash.Count == 0)
        {
            _sceneTracker.stageCleared = true;
            CalculateStageScore();
        }
    }

    private void CalculateStageScore()
    {
        _tankReserveEmpty = false;
        if (_sceneTracker.stageCleared)
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            _sceneTracker.playerLevel = player.playerLevel;
        }
        SceneManager.LoadScene("Score");
    }

    // needs to refactore
    private IEnumerator FreezeActivated()
    {
        EnemyAI.freezing = true;

        for (int i = 0; i <= enemyStash.Count - 1; i++)
        {
            enemyStash[i].ToFreezeTank();
        }
        yield return new WaitForSeconds(10);

        for (int i = 0; i <= enemyStash.Count - 1; i++)
        {
            enemyStash[i].ToUnfreezeTank();
        }

        EnemyAI.freezing = false;
    }

    private bool InvalidBonusCratePosition(Vector3 cratePosition)
    {
        return _waterTilemap.GetTile(_waterTilemap.WorldToCell(cratePosition)) != null || _steelTilemap.GetTile(_steelTilemap.WorldToCell(cratePosition)) != null;
    }

    private IEnumerator StartSpawningEnemy()
    {
        yield return new WaitForSeconds(1);
        InvokeRepeating("SpawnEnemy", _levelManager.spawnRate, _levelManager.spawnRate); //it will call SpawnEnemy after 5 seconds and repeat the invoke every 5 seconds.
        SpawnPlayer();
    }
}