using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerCore playerCore, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine psm, WeaponCore weaponCore) : base(playerCore, movement, input, camera, psm, weaponCore) { }
    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
        weaponCore.CanQueueAttack = true;
        weaponCore.CanAttack = true;
        //movement.AnimationTriggerer("Idle");
    }
    public override void Tick()
    {
        if(input.MoveInput.sqrMagnitude > 0.01f) playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, camera, playerStateMachine, weaponCore));
        if(input.Crouch) playerStateMachine.SwitchState(new CrouchState(playerCore, movement, input, camera, playerStateMachine, weaponCore));
        //if(input.Attack) playerStateMachine.SwitchState(new AttackState(playerCore, movement, input, camera, playerStateMachine, weaponCore));

        camera.RotationManager(input.LookInput);
        camera.PlayerRotManager(movement.RefinedMovementDirection);

        if (playerCore.currentStamina < playerCore.maxStamina) playerCore.currentStamina += (playerCore.staminaRegenRate * playerCore.staminaRegenRateMuliplier * playerCore.idleStaminaRegenMultiplier * Time.deltaTime);
    }
}
