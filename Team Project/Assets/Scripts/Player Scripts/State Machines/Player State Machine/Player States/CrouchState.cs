using UnityEngine;

public class CrouchState : PlayerState
{
    public CrouchState(PlayerCore playerCore, PlayerMovement movement, PlayerInputManager input, PlayerStateMachine psm, WeaponCore weaponCore, TargetLockHandler targetLock, EquipManager equipManager) : base(playerCore, movement, input, psm, weaponCore, targetLock, equipManager) { }

    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);     
        if(!playerCore.isCrouching) movement.Crouch();
    }

    public override void Tick()
    {
        if (!input.Crouch && movement.CheckifCanUncrouch())
        {
            playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
            movement.UnCrouch();
        }
        
        if(input.MoveInput.sqrMagnitude > 0.01f) playerStateMachine.SwitchState(new CrouchWalkState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
        
        if (playerCore.currentStamina < playerCore.maxStamina) playerCore.currentStamina += (playerCore.staminaRegenRate * playerCore.crouchStaminaRegenMultiplier * playerCore.staminaRegenRateMuliplier * Time.deltaTime);

    }
}
