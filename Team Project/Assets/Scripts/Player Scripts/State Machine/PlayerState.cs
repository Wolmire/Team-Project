public abstract class PlayerState
{
    protected PlayerCore core;
    protected PlayerMovement movement;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerInput input;
    protected PlayerCamera camera;

    public PlayerState(PlayerCore core, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine sm)
    {
        this.core = core;
        this.movement = movement;
        this.input = input;
        this.playerStateMachine = sm;
        this.camera = camera;
    }

    public virtual void Enter() { }
    public virtual void Tick() { }
    public virtual void FixedTick() { }
    public virtual void Exit() { }
}
