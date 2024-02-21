using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class TankAbstract<T> : MonoBehaviour where T : SO_GenericTank
{
    [SerializeField] protected T tankData;
    protected GamePlayManager gamePlayManager;
    protected SceneTracker sceneTracker;
    protected AudioManager audioManager;
    protected AnimationController animationController;
    protected Health healthController;
    protected WeaponController weaponController;
    protected LayerMask blockingLayer;
    protected List<Vector2> availableDirections;
    protected List<Vector2> possibleDirections;
    protected Vector2 currentDirection;
    protected float speed;
    private Collider2D _obstacleCollider;
    private Vector2 _colliderSize;
    private Vector2 _boxCastPosition;
    private Quaternion _rotation;
    private float _boxHeight = 0.05f;
    private float _offset;

    [Inject]
    private void Construct(SceneTracker sceneTracker, GamePlayManager gamePlayManager,AudioManager audioManager)
    {
        this.sceneTracker = sceneTracker;
        this.gamePlayManager = gamePlayManager;
        this.audioManager = audioManager;
    }

    protected virtual void Start()
    {
        _colliderSize = GetComponent<BoxCollider2D>().size;
        animationController = new AnimationController(GetComponent<Animator>());
        healthController = GetComponent<Health>();
        healthController.InitializeWithData(tankData, animationController);
        weaponController = GetComponentInChildren<WeaponController>();
        weaponController.InitializeWithData(tankData, animationController);
        InitializeWithData();
    }

    protected virtual void InitializeWithData()
    {
        possibleDirections = new List<Vector2>();
        availableDirections = new List<Vector2>();
        possibleDirections.Add(Vector2.up);
        possibleDirections.Add(Vector2.down);
        possibleDirections.Add(Vector2.right);
        possibleDirections.Add(Vector2.left);
    }

    protected void Move(Vector2 direction)
    {
        if (CheckAvailableDirection(direction))
        {
            if (direction.x != 0)
                MoveHorizontal(direction.x);
            else if (direction.y != 0)
                MoveVertical(direction.y);
        }
        else
        {
            if (direction.x != 0)
                RotateHorizontal(direction.x);
            else if (direction.y != 0)
                RotateVertical(direction.y);
        }
        currentDirection = direction;
    }

    protected void RotateVertical(float _vertical)
    {
        if (_vertical < 0)
            _rotation = Quaternion.Euler(0, 0, _vertical * 180f);
        else
            _rotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = _rotation;
    }

    protected void RotateHorizontal(float _horizontal)
    {
        _rotation = Quaternion.Euler(0, 0, -_horizontal * 90f);
        transform.rotation = _rotation;
    }

    protected void MoveVertical(float _vertical)
    {
        RotateVertical(_vertical);
        Vector3 movement = new Vector3(0f, speed * Time.deltaTime, 0f);
        transform.Translate(movement);
    }

    protected void MoveHorizontal(float _horizontal)
    {
        RotateHorizontal(_horizontal);
        Vector3 movement = new Vector3(0f, speed * Time.deltaTime, 0f);
        transform.Translate(movement);
    }

    protected void GetBoxCastPosition()
    {
        _boxCastPosition = new Vector2(transform.position.x, transform.position.y);
        _offset = (_colliderSize.x / 2f) + (_boxHeight / 2) + _boxHeight;
    }

    protected bool CheckAvailableDirection(Vector2 direction)
    {
        availableDirections.Clear();
        GetBoxCastPosition();

        switch (direction)
        {
            case Vector2 v when v.Equals(Vector2.up):
                _obstacleCollider = Physics2D.OverlapBox(_boxCastPosition + Vector2.up * _offset, new Vector2(_colliderSize.x - 0.1f, _boxHeight), transform.rotation.z, blockingLayer);
                break;
            case Vector2 v when v.Equals(-Vector2.up):
                _obstacleCollider = Physics2D.OverlapBox(_boxCastPosition - Vector2.up * _offset, new Vector2(_colliderSize.x - 0.1f, _boxHeight), transform.rotation.z, blockingLayer);
                break;
            case Vector2 v when v.Equals(Vector2.right):
                _obstacleCollider = Physics2D.OverlapBox(_boxCastPosition + Vector2.right * _offset, new Vector2(_boxHeight, _colliderSize.y - 0.1f), transform.rotation.z, blockingLayer);
                break;
            case Vector2 v when v.Equals(-Vector2.right):
                _obstacleCollider = Physics2D.OverlapBox(_boxCastPosition - Vector2.right * _offset, new Vector2(_boxHeight, _colliderSize.y - 0.1f), transform.rotation.z, blockingLayer);
                break;
        }
        if (!_obstacleCollider) availableDirections.Add(-Vector2.up);

        return !_obstacleCollider ? true : false;
    }

    protected void CheckAllAvailableDirections()
    {
        availableDirections.Clear();
        GetBoxCastPosition();

        foreach (var possibleDirection in possibleDirections)
        {
            if (possibleDirection.x != 0)
                _obstacleCollider = Physics2D.OverlapBox(_boxCastPosition + _offset * possibleDirection, new Vector2(_colliderSize.x - 0.1f, _boxHeight), transform.rotation.z, blockingLayer);
            else if (possibleDirection.y != 0)
                _obstacleCollider = Physics2D.OverlapBox(_boxCastPosition + _offset * possibleDirection, new Vector2(_boxHeight, _colliderSize.y - 0.1f), transform.rotation.z, blockingLayer);

            if (!_obstacleCollider) availableDirections.Add(possibleDirection);
        }
    }

    protected virtual void OnDrawGizmos()
    {
        DrawObstacles();
    }

    private void DrawObstacles()
    {
        GetBoxCastPosition();
        // VERTICAL
        _obstacleCollider = Physics2D.OverlapBox(_boxCastPosition + Vector2.up * _offset, new Vector2(_colliderSize.x - 0.1f, _boxHeight), transform.rotation.z, blockingLayer);
        if (_obstacleCollider)
        {
            Gizmos.color = Color.red; // up
            Gizmos.DrawWireCube(_boxCastPosition + Vector2.up * _offset, new Vector2(_colliderSize.x - 0.1f, _boxHeight));
        }

        _obstacleCollider = Physics2D.OverlapBox(_boxCastPosition - Vector2.up * _offset, new Vector2(_colliderSize.x - 0.1f, _boxHeight), transform.rotation.z, blockingLayer);
        if (_obstacleCollider)
        {
            Gizmos.color = Color.green; // down 
            Gizmos.DrawWireCube(_boxCastPosition - Vector2.up * _offset, new Vector2(_colliderSize.x - 0.1f, _boxHeight));
        }

        // HORIZONTAL
        _obstacleCollider = Physics2D.OverlapBox(_boxCastPosition + Vector2.right * _offset, new Vector2(_boxHeight, _colliderSize.y - 0.1f), transform.rotation.z, blockingLayer);
        if (_obstacleCollider)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_boxCastPosition + Vector2.right * _offset, new Vector2(_boxHeight, _colliderSize.y - 0.1f));
        }

        _obstacleCollider = Physics2D.OverlapBox(_boxCastPosition - Vector2.right * _offset, new Vector2(_boxHeight, _colliderSize.y - 0.1f), transform.rotation.z, blockingLayer);
        if (_obstacleCollider)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_boxCastPosition - Vector2.right * _offset, new Vector2(_boxHeight, _colliderSize.y - 0.1f));
        }
    }
}