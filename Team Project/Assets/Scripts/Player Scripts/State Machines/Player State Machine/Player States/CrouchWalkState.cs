using UnityEngine;

public class CrouchWalkState : PlayerState
{
    public CrouchWalkState(PlayerCore playerCore, PlayerMovement movement, PlayerInputManager input, PlayerStateMachine psm, WeaponCore weaponCore, TargetLockHandler targetLock, PlayerEquipManager equipManager) : base(playerCore, movement, input, psm, weaponCore, targetLock, equipManager) { }

    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);

        if(!playerCore.isCrouching) movement.Crouch();
    }
    public override void Tick()
    {
        if (input.MoveInput.sqrMagnitude < 0.01f) playerStateMachine.SwitchState(new CrouchState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));

        movement.Move(input.MoveInput, targetLock.activeTarget, targetLock.GetActiveCamera(), targetLock.currentTarget);
        movement.ApplyMovement(movement.CrouchSpeedMultiplier);


        if (input.Sprint && playerCore.currentStamina > playerCore.runMinStamina && movement.CheckifCanUncrouch()) 
        {
            playerStateMachine.SwitchState(new RunState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
            movement.UnCrouch();
        }

        if (!input.Crouch && movement.CheckifCanUncrouch())
        {
            playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
            movement.UnCrouch();
        }

        if (playerCore.currentStamina < playerCore.maxStamina) playerCore.currentStamina += (playerCore.staminaRegenRate * playerCore.crouchWalkStaminaRegenMultiplier * playerCore.staminaRegenRateMuliplier * Time.deltaTime);

    }
}
