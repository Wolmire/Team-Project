public abstract class PlayerState
{
    protected PlayerCore core;
    protected MVMDefault movement;
    protected PlayerStateMachine stateMachine;

    public PlayerState(PlayerCore core, MVMDefault movement, PlayerStateMachine sm)
    {
        this.core = core;
        this.movement = movement;
        stateMachine = sm;
    }

    public virtual void Enter() { }
    public virtual void Tick() { }
    public virtual void FixedTick() { }
    public virtual void Exit() { }
}
