using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class RunState : PlayerState
{
    public RunState(PlayerCore playerCore, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine psm, WeaponCore weaponCore) : base(playerCore, movement, input, camera, psm, weaponCore) {}
    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
    }

    public override void Tick()
    {
        if (!input.Sprint) playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, camera, playerStateMachine, weaponCore));
        if (input.MoveInput.sqrMagnitude < 0.01f) playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, camera, playerStateMachine, weaponCore));
        if (!movement.isGrounded()) playerStateMachine.SwitchState(new FallState(playerCore, movement, input, camera, playerStateMachine, weaponCore));

        if (input.JumpPressed())
        {
            movement.AirSpeedMultiplier = movement.RunSpeedMultiplier;
            playerStateMachine.SwitchState(new JumpState(playerCore, movement, input, camera, playerStateMachine, weaponCore));
        }

        movement.Move(input.MoveInput, camera.Locked, camera.Camera);
        movement.ApplyMovement(movement.RunSpeedMultiplier);

        camera.RotationManager(input.LookInput);
        camera.PlayerRotManager(movement.RefinedMovementDirection);
        
        playerCore.currentStamina -= playerCore.runStaminaCost * Time.deltaTime;

        if (playerCore.currentStamina <= 0)
        {
            playerCore.currentStamina = 0;

            playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, camera, playerStateMachine, weaponCore));
        }
    }

    public override void Exit()
    {
        movement.AirSpeedMultiplier = movement.RunSpeedMultiplier;
    }
}
