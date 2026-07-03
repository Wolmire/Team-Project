public abstract class PlayerState
{
    protected PlayerCore playerCore;
    protected PlayerMovement movement;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerInputManager input;
    protected WeaponCore weaponCore;
    protected TargetLockHandler targetLock;

    public PlayerState(PlayerCore playerCore, PlayerMovement movement, PlayerInputManager input, PlayerStateMachine psm, WeaponCore weaponCore, TargetLockHandler targetLock)
    {
        this.playerCore = playerCore;
        this.movement = movement;
        this.input = input;
        this.playerStateMachine = psm;
        this.weaponCore = weaponCore;
        this.targetLock = targetLock;
    }


    public virtual void Enter() { }
    public virtual void Tick() { }
    public virtual void FixedTick() { }
    public virtual void Exit() { }
}
