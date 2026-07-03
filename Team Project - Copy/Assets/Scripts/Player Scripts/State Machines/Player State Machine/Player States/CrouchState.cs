using UnityEngine;

public class CrouchState : PlayerState
{
    public CrouchState(PlayerCore playerCore, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine psm, WeaponStateMachine wsm, WeaponCore weaponCore) : base(playerCore, movement, input, camera, psm, wsm, weaponCore) { }

    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);     
        movement.Crouch();
        weaponCore.CanAttack = false;
    }

    public override void Tick()
    {
        if(!input.Crouch) playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, camera, playerStateMachine, weaponStateMachine, weaponCore));
    }

    public override void Exit()
    {
        movement.UnCrouch();        
    }
}
