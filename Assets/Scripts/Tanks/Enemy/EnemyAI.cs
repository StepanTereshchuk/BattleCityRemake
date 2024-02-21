using System.Collections;
using UnityEngine;

public class EnemyAI : TankAbstract<SO_EnemyTank>
{
    public static bool freezing;
    private float _minTimeBeforeFire;
    private float _maxTimeBeforeFire;
    private float _minTimeBeforeChangeDirection;
    private float _maxTimeBeforeChangeDirection;
    private float _timeBeforeTurn;
    private float _timeBeforeFire;
    private DirectionRandomizer _randomizer;
    private IEnumerator DirectionTimer;
    private IEnumerator FireTimer;

    public void ToFreezeTank()
    {
        StopAllCoroutines();
        currentDirection = Vector2.zero;
    }

    public void ToUnfreezeTank()
    {
        StartCoroutine(DirectionTimer);
        StartCoroutine(FireTimer);
    }

    private void RandomDirection()
    {
        GenerateTimeDelay(_minTimeBeforeChangeDirection, _maxTimeBeforeChangeDirection, ref _timeBeforeTurn);
        currentDirection = Vector2.zero;
        CheckAllAvailableDirections();
        currentDirection = _randomizer.GetRandomDirection(possibleDirections);
    }

    private void FireWhenWanted()
    {
        GenerateTimeDelay(_minTimeBeforeFire, _maxTimeBeforeFire, ref _timeBeforeFire);
        weaponController.Fire();
    }

    private void GenerateTimeDelay(float min, float max, ref float timer)
    {
        timer = Random.Range(min, max);
    }

    private IEnumerator DirectionTimerTick()
    {
        while (_timeBeforeTurn > 0)
        {
            --_timeBeforeTurn;
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator FireTimerTick()
    {
        while (_timeBeforeFire > 0)
        {
            --_timeBeforeFire;
            yield return new WaitForSeconds(1);
        }
    }

    protected override void InitializeWithData()
    {
        base.InitializeWithData();
        blockingLayer = tankData.BlockingLayerMask;
        speed = tankData.Speed;
        _minTimeBeforeFire = tankData.MinTimeBeforeFire;
        _maxTimeBeforeFire = tankData.MaxTimeBeforeFire;
        _minTimeBeforeChangeDirection = tankData.MinTimeBeforeChangeDirection;
        _maxTimeBeforeChangeDirection = tankData.MaxTimeBeforeChangeDirection;
    }

    protected override void Start()
    {
        base.Start();
        _randomizer = new DirectionRandomizer();
        DirectionTimer = DirectionTimerTick();
        FireTimer = FireTimerTick();

        if (!freezing)
        {
            StartCoroutine(DirectionTimer);
            StartCoroutine(FireTimer);
        }
        animationController.Spawn();
        //tankAnimator.SetTrigger("Spawning");
    }

    private void FixedUpdate()
    {
        if (_timeBeforeFire <= 0)
        {
            FireWhenWanted();
        }
        else if (_timeBeforeTurn <= 0 || currentDirection != Vector2.zero && !CheckAvailableDirection(currentDirection))
        {
            RandomDirection();
        }

        if (!currentDirection.Equals(Vector2.zero))
        {
            Move(currentDirection);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            currentDirection = Vector2.zero;
            FireWhenWanted();
            RandomDirection();
        }
        else
        {
            RandomDirection();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        GenerateTimeDelay(_minTimeBeforeChangeDirection, _minTimeBeforeChangeDirection, ref _timeBeforeTurn);
        GenerateTimeDelay(_minTimeBeforeFire, _maxTimeBeforeFire, ref _timeBeforeFire);

        if (DirectionTimer != null && FireTimer != null)
        {
            StartCoroutine(DirectionTimer);
            StartCoroutine(FireTimer);
        }
    }

    private void OnDestroy()
    {
        gamePlayManager.DecreaseStash(this);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}