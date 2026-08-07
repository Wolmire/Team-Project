using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
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

        if (input.PrimaryInputPressed() && equipManager.CurrentWeapon)
        {
            equipManager.CycleInventory(PlayerEquipManager.Hand.MainHand);
            weaponCore.CurrentWeaponData = equipManager.CurrentWeapon.WeaponData;
        }
        if (input.SecondaryInputPressed() && equipManager.CurrentWeapon)
        {
            equipManager.CycleInventory(PlayerEquipManager.Hand.OffHand);
            weaponCore.CurrentAltWeaponData = equipManager.CurrentAltWeapon.WeaponData;
        }

        if (weaponCore.CurrentWeaponData.Magic)
        {
            Debug.Log("UsingMagic");
            HandleMagicAttackInput();
        }
        else
        {
            Debug.Log("Melee");
            HandleMeleeAttackInput();
        }
    }

    private void HandleMagicAttackInput()
    {
        if (input.LightAttackPressed()) InitiateMelee(attackChainType.Light, weaponCore.CurrentWeaponData.lightAttacks);

        if (input.HeavyAttackPressed()) InitiateMagic(attackChainType.Heavy, weaponCore.MagicSpells);

        if (input.SpecialAttackPressed()) InitiateMagic(attackChainType.Special, weaponCore.CurrentWeaponData.specialAttacks);
    }
    private void HandleMeleeAttackInput()
    {
        if (input.LightAttackPressed()) InitiateMelee(attackChainType.Light, weaponCore.CurrentWeaponData.lightAttacks);

        if (input.HeavyAttackPressed()) InitiateMelee(attackChainType.Heavy, weaponCore.CurrentWeaponData.heavyAttacks);

        if (input.SpecialAttackPressed()) InitiateMelee(attackChainType.Special, weaponCore.CurrentWeaponData.specialAttacks);
    }

    private void InitiateMagic(attackChainType type, WeaponAttack[] list)
    {
        if (list == null || list.Length == 0) return;
        WeaponAttack firstAttack = list[0];
        if (playerCore.currentStamina < firstAttack.staminaCost) return;
        weaponCore.attackChainIndex = 0;
        weaponCore.currentChainAttackType = type;
        weaponCore.QueueAttack(firstAttack, type, 0);
        playerStateMachine.SwitchState(new AttackState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));

    }

    private void InitiateMelee(attackChainType type, WeaponAttack[] list) //basically checks if player has enough stamina and there is a valid attack to initiate
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
