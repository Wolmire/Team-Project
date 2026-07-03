using UnityEngine;

public class FallState : PlayerState
{
    public FallState(PlayerCore playerCore, PlayerMovement movement, PlayerInputManager input, PlayerStateMachine psm, WeaponCore weaponCore, TargetLockHandler targetLock) : base(playerCore, movement, input, psm, weaponCore, targetLock) { }

    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
    }

    public override void Tick()
    {
        if (movement.isGrounded()) playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock));

        movement.ApplyMovement(movement.AirSpeedMultiplier);
    }
}
