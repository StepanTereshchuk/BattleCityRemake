public abstract class State
{
    protected StateMachine stateMachine;

    public State(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState()
    {

    }
    public virtual void PhysicUpdate()
    {

    }
    public virtual void Update()
    {

    }
    public virtual void ExitState()
    {

    }
}