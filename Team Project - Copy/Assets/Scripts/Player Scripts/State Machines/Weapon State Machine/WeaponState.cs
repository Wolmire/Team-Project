using UnityEngine;

public abstract class WeaponState
{
    protected WeaponStateMachine WeaponStateMachine;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerInput input;
    protected WeaponCore weaponCore;
    public WeaponState(PlayerInput input, WeaponCore weaponCore, WeaponStateMachine wsm, PlayerStateMachine psm)
    {
        this.input = input;
        this.weaponCore = weaponCore;
        this.WeaponStateMachine = wsm;
        this.playerStateMachine = psm;
    }

    public virtual void Enter() { }
    public virtual void Tick() { }
    public virtual void FixedTick() { }
    public virtual void Exit() { }
}
