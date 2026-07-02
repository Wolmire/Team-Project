using UnityEngine;

public class FallState : PlayerState
{
    public FallState(PlayerCore playerCore, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine psm, WeaponCore weaponCore) : base(playerCore, movement, input, camera, psm, weaponCore) { }

    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
    }

    public override void Tick()
    {
        if (movement.isGrounded()) playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, camera, playerStateMachine, weaponCore));

        movement.ApplyMovement(movement.AirSpeedMultiplier);
        camera.RotationManager(input.LookInput);
    }
}
