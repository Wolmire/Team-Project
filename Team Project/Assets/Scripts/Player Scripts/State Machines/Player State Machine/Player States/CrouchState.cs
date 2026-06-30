using UnityEngine;

public class CrouchState : PlayerState
{
    public CrouchState(PlayerCore playerCore, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine psm, WeaponCore weaponCore) : base(playerCore, movement, input, camera, psm, weaponCore) { }

    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);     
        if(!playerCore.isCrouching) movement.Crouch();
    }

    public override void Tick()
    {
        if (!input.Crouch)
        {
            playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, camera, playerStateMachine, weaponCore));
            movement.UnCrouch();
        }
        
        if(input.MoveInput.sqrMagnitude > 0.01f) playerStateMachine.SwitchState(new CrouchWalkState(playerCore, movement, input, camera, playerStateMachine, weaponCore));
        
        if (playerCore.currentStamina < playerCore.maxStamina) playerCore.currentStamina += (playerCore.staminaRegenRate * playerCore.crouchStaminaRegenMultiplier * playerCore.staminaRegenRateMuliplier * Time.deltaTime);

    }
}
