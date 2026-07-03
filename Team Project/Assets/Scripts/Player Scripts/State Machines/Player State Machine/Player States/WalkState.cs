using UnityEngine;

public class WalkState : PlayerState
{
    public WalkState(PlayerCore playerCore, PlayerMovement movement, PlayerInputManager input, PlayerStateMachine psm, WeaponCore weaponCore, TargetLockHandler targetLock) : base(playerCore, movement, input, psm, weaponCore, targetLock) { }
    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
        movement.AnimationBool("Walking", true);
    }
    public override void Tick()
    {
        if (input.MoveInput.sqrMagnitude < 0.01f) playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock));
        if (input.Sprint && playerCore.currentStamina > playerCore.runMinStamina) playerStateMachine.SwitchState(new RunState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock));
        if (input.Crouch) playerStateMachine.SwitchState(new CrouchWalkState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock));
        if (!movement.isGrounded()) playerStateMachine.SwitchState(new FallState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock));
        if (input.JumpPressed())
        {
            playerStateMachine.SwitchState(new JumpState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock));
        }

        movement.Move(input.MoveInput, targetLock.activeTarget, targetLock.GetActiveCamera(), targetLock.currentTarget);
        movement.ApplyMovement(movement.WalkSpeedMultiplier);

        if(playerCore.currentStamina < playerCore.maxStamina) playerCore.currentStamina += (playerCore.staminaRegenRate * playerCore.walkStaminaRegenMultiplier * playerCore.staminaRegenRateMuliplier * Time.deltaTime);
        HandleAttackInput();
    }
    private void HandleAttackInput()
    {
        if (input.LightAttackPressed()) InitiateAttack(attackChainType.Light, weaponCore.CurrentWeaponData.lightAttacks);

        if (input.HeavyAttackPressed()) InitiateAttack(attackChainType.Heavy, weaponCore.CurrentWeaponData.heavyAttacks);

        if (input.SpecialAttackPressed()) InitiateAttack(attackChainType.Special, weaponCore.CurrentWeaponData.specialAttacks);
    }

    private void InitiateAttack(attackChainType type, WeaponAttack[] list) //basically checks if player has enough stamina and there is a valid attack to initiate
    {
        if (list == null || list.Length == 0) return;
        WeaponAttack firstAttack = list[0];
        if (playerCore.currentStamina < firstAttack.staminaCost) return;
        weaponCore.attackChainIndex = 0;
        weaponCore.currentChainAttackType = type;
        weaponCore.QueueAttack(firstAttack, type, 0);
        playerStateMachine.SwitchState(new AttackState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock));
    }

    public override void Exit()
    {
        movement.AirSpeedMultiplier = movement.WalkSpeedMultiplier;
        movement.AnimationBool("Walking", false);
    }
}
