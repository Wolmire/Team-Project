using UnityEngine;
using UnityEngine.InputSystem;
public class IdleState : PlayerState
{
    public IdleState(PlayerCore playerCore, PlayerMovement movement, PlayerInputManager input, PlayerStateMachine psm, WeaponCore weaponCore, TargetLockHandler targetLock, PlayerEquipManager equipManager) : base(playerCore, movement, input, psm, weaponCore, targetLock, equipManager) { }
    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
        movement.AnimationBool("Idle", true);
    }
    public override void Tick()
    {
        if (input.MoveInput.sqrMagnitude > 0.01f) playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));

        if (input.Crouch) playerStateMachine.SwitchState(new CrouchState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));

        if(!movement.isGrounded()) playerStateMachine.SwitchState(new FallState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));

        if (input.JumpPressed()) playerStateMachine.SwitchState(new JumpState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));

        if (playerCore.currentStamina < playerCore.maxStamina) playerCore.currentStamina += playerCore.staminaRegenRate * playerCore.staminaRegenRateMuliplier * playerCore.idleStaminaRegenMultiplier * Time.deltaTime;

        movement.ApplyMovement(0);

        if(input.TargetLockPressed()) targetLock.TargetLock(!targetLock.activeTarget);
        targetLock.HandleSwitchTargetInput(input.DeviceType, input.LookInput.x);

        if (input.PrimaryInputPressed() && equipManager.CurrentWeapon != equipManager.WeaponSlot1)
        {
            equipManager.Equip(equipManager.WeaponSlot1, equipManager.RSlot);
            weaponCore.CurrentWeaponData = equipManager.CurrentWeapon.WeaponData;
        }
        if (input.SecondaryInputPressed() && equipManager.CurrentWeapon != equipManager.WeaponSlot2)
        {
            equipManager.Equip(equipManager.WeaponSlot2, equipManager.RSlot);
            weaponCore.CurrentWeaponData = equipManager.CurrentWeapon.WeaponData;
        }
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
        playerStateMachine.SwitchState(new AttackState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
    }
    public override void Exit()
    {
        movement.AirSpeedMultiplier = 0.5f;
        movement.AnimationBool("Idle", false);
    }
}
