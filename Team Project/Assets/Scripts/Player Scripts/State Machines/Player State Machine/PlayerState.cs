public abstract class PlayerState
{
    protected PlayerCore playerCore;
    protected PlayerMovement movement;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerInput input;
    protected PlayerCamera camera;
    protected WeaponStateMachine weaponStateMachine;
    protected WeaponCore weaponCore;

    public PlayerState(PlayerCore playerCore, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine psm, WeaponStateMachine wsm, WeaponCore weaponCore)
    {
        this.playerCore = playerCore;
        this.movement = movement;
        this.input = input;
        this.playerStateMachine = psm;
        this.camera = camera;
        this.weaponStateMachine = wsm;
        this.weaponCore = weaponCore;
    }

    public virtual void Enter() { }
    public virtual void Tick() { }
    public virtual void FixedTick() { }
    public virtual void Exit() { }
}
