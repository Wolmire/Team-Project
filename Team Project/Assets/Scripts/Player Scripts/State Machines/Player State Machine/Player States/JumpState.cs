using JetBrains.Annotations;
using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerCore playerCore, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine psm, WeaponCore weaponCore) : base(playerCore, movement, input, camera, psm, weaponCore) { }

    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
        movement.Jump();    
    }

    public override void Tick()
    {
        movement.ApplyMovement(movement.AirSpeedMultiplier);
        camera.RotationManager(input.LookInput);

        if (movement.MController.velocity.y < -0.1f)
        {
            playerStateMachine.SwitchState(new FallState(playerCore, movement, input, camera, playerStateMachine, weaponCore));
        }
    }
}
