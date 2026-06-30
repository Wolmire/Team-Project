using UnityEngine;

public class WalkState : PlayerState
{
    public WalkState(PlayerCore playerCore, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine psm, WeaponCore weaponCore) : base(playerCore, movement, input, camera, psm, weaponCore) { }
    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
        movement.AnimationTriggerer("Walking");
    }
    public override void Tick()
    {
        if (input.MoveInput.sqrMagnitude < 0.01f) playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, camera, playerStateMachine, weaponCore));
        if (input.Sprint && playerCore.currentStamina > playerCore.runMinStamina) playerStateMachine.SwitchState(new RunState(playerCore, movement, input, camera, playerStateMachine, weaponCore));
        if (input.Crouch) playerStateMachine.SwitchState(new CrouchWalkState(playerCore, movement, input, camera, playerStateMachine, weaponCore));

        movement.Move(input.MoveInput, camera.Locked, camera.Camera);
        playerCore.MController.Move(movement.RefinedMovementDirection.normalized * movement.MovementSpeed * movement.WalkSpeedMultiplier * Time.deltaTime);

        camera.RotationManager(input.LookInput);
        camera.PlayerRotManager(movement.RefinedMovementDirection);

        if(playerCore.currentStamina < playerCore.maxStamina) playerCore.currentStamina += (playerCore.staminaRegenRate * playerCore.walkStaminaRegenMultiplier * playerCore.staminaRegenRateMuliplier * Time.deltaTime);
    }
}
