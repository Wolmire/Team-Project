using UnityEngine;

public class CrouchWalkState : PlayerState
{
    public CrouchWalkState(PlayerCore playerCore, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine psm, WeaponCore weaponCore) : base(playerCore, movement, input, camera, psm, weaponCore) { }

    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);

        if(!playerCore.isCrouching) movement.Crouch();
    }
    public override void Tick()
    {
        if (input.MoveInput.sqrMagnitude < 0.01f) playerStateMachine.SwitchState(new CrouchState(playerCore, movement, input, camera, playerStateMachine, weaponCore));

        movement.Move(input.MoveInput, camera.Locked, camera.Camera);
        playerCore.MController.Move(movement.RefinedMovementDirection.normalized * movement.MovementSpeed * movement.CrouchSpeedMultiplier * Time.deltaTime);

        camera.RotationManager(input.LookInput);
        camera.PlayerRotManager(movement.RefinedMovementDirection);

        if (input.Sprint && playerCore.currentStamina > playerCore.runMinStamina) 
        {
            playerStateMachine.SwitchState(new RunState(playerCore, movement, input, camera, playerStateMachine, weaponCore));
            movement.UnCrouch();
        }

        if (!input.Crouch)
        {
            playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, camera, playerStateMachine, weaponCore));
            movement.UnCrouch();
        }

        if (playerCore.currentStamina < playerCore.maxStamina) playerCore.currentStamina += (playerCore.staminaRegenRate * playerCore.crouchWalkStaminaRegenMultiplier * playerCore.staminaRegenRateMuliplier * Time.deltaTime);

    }
}
