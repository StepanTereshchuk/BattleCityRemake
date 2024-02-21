using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public enum States
    {
        Move,
        Shoot,
        Retreat,
        Chase,
        Die
    }
    public EnemyTank tank { get; private set; }
    public LayerMask blockingLayer { get; private set; }
    private Dictionary<States, State> _states;
    private State _currentState;
    private Animator _animator;
    string stateName = "Idle";

    public StateMachine(EnemyTank tank, LayerMask blockingLayer)
    {
        _states = new Dictionary<States, State>();
        _states.Add(States.Move, new MoveState(this));
        _states.Add(States.Shoot, new ShootState(this));
        this.blockingLayer = blockingLayer;
        this.tank = tank;
    }

    public void SetCurrentState(States newState)
    {
        _currentState = _states.GetValueOrDefault(newState);
        _currentState.EnterState();
    }


    public void Execute()
    {
        _currentState.PhysicUpdate();
    }

    public void Update()
    {
        switch (stateName)
        {
            case "Attack":
                _animator.SetTrigger("Attack"); break;
            case "Take Cover":
                _animator.SetTrigger("Take Cover"); break;
            case "Reload":
                _animator.SetTrigger("Reload"); break;
            case "Patrol":
                _animator.SetBool("Walk",true); break;
            case "Dead":
                _animator.SetTrigger("Dead"); break;
        }

    }
}