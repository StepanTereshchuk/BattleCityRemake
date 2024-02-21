using UnityEngine;
using UnityEngine.Events;

public class EnemyTank : MonoBehaviour
{
    [SerializeField] private LayerMask blockingLayer;
    private StateMachine _stateMachine;
    public UnityEvent onCollisionEvent;

    private void Start()
    {
        onCollisionEvent = new UnityEvent();
        _stateMachine = new StateMachine(this, blockingLayer);
        _stateMachine.SetCurrentState(StateMachine.States.Move);
    }
    private void Update()
    {
        if (_stateMachine != null)
            _stateMachine.Execute();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(" --  OnTriggerEnter2D");
        onCollisionEvent.Invoke();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(" --  OnTrigger STAY 2D");
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log(" --  OnTriggerExit2D");
    }
}
