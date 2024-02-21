using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MoveState : State
{
    private float _horizontal;
    private float _vertical;
    private float _speed = 2f;
    private enum Direction { Up, Down, Left, Right };
    private IEnumerator coroutine;
    private float timeBeforeTurn = 5f;

    public MoveState(StateMachine stateMachine) : base(stateMachine)
    {
        coroutine = ChangeDirection();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        if (_vertical != 0) MoveVertical();
        else if (_horizontal != 0) MoveHorizontal();
    }

    public override void EnterState()
    {
        base.EnterState();
        stateMachine.tank.StartCoroutine(coroutine);
        stateMachine.tank.onCollisionEvent.AddListener(RandomDirection);
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exit MOVE State");
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            RandomDirection();
            yield return new WaitForSeconds(timeBeforeTurn);
        }

    }

    private void RandomDirection()
    {
        List<Direction> lottery = new List<Direction>();
        ////////
        if (!Physics2D.Linecast(stateMachine.tank.transform.position, (Vector2)stateMachine.tank.transform.position + new Vector2(1, 0), stateMachine.blockingLayer))
        {
            lottery.Add(Direction.Right);
        }
        if (!Physics2D.Linecast(stateMachine.tank.transform.position, (Vector2)stateMachine.tank.transform.position + new Vector2(-1, 0), stateMachine.blockingLayer))
        {
            lottery.Add(Direction.Left);
        }
        if (!Physics2D.Linecast(stateMachine.tank.transform.position, (Vector2)stateMachine.tank.transform.position + new Vector2(0, 1), stateMachine.blockingLayer))
        {
            lottery.Add(Direction.Up);
        }
        if (!Physics2D.Linecast(stateMachine.tank.transform.position, (Vector2)stateMachine.tank.transform.position + new Vector2(0, -1), stateMachine.blockingLayer))
        {
            lottery.Add(Direction.Down);
        }
        if (lottery.Count > 0)
        {
            Direction selection = lottery[Random.Range(0, lottery.Count)];

            if (selection == Direction.Up)
            {
                _vertical = 1;
                _horizontal = 0;
            }
            if (selection == Direction.Down)
            {
                _vertical = -1;
                _horizontal = 0;
            }
            if (selection == Direction.Right)
            {
                _vertical = 0;
                _horizontal = 1;
            }
            if (selection == Direction.Left)
            {
                _vertical = 0;
                _horizontal = -1;
            }

        }
    }
    
    private void MoveVertical()
    {
        Debug.Log("VERTICAL:" + _vertical);
        Quaternion rotation;
        if (_vertical < 0)
        {
            rotation = Quaternion.Euler(0, 0, _vertical * 180f);
        }
        else
        {
            rotation = Quaternion.Euler(0, 0, 0);
        }
        stateMachine.tank.transform.rotation = rotation;

        Vector3 movement = new Vector3(0f, _speed * Time.deltaTime, 0f);
        stateMachine.tank.transform.Translate(movement);
    }

    private void MoveHorizontal()
    {
        Debug.Log("Horizontal:" + _horizontal);
        Quaternion rotation = Quaternion.Euler(0, 0, -_horizontal * 90f);
        stateMachine.tank.transform.rotation = rotation;

        Vector3 movement = new Vector3(0f, _speed * Time.deltaTime, 0f);
        stateMachine.tank.transform.Translate(movement);
    }
}