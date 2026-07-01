using UnityEngine;
using UnityEngine.InputSystem;
public class IdleState : PlayerState
{
    public IdleState(PlayerCore playerCore, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine psm, WeaponCore weaponCore) : base(playerCore, movement, input, camera, psm, weaponCore) { }
    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
        //movement.AnimationTriggerer("Idle"); 

    }
    public override void Tick()
    {
        if (input.MoveInput.sqrMagnitude > 0.01f) playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, camera, playerStateMachine, weaponCore));

        if (input.Crouch) playerStateMachine.SwitchState(new CrouchState(playerCore, movement, input, camera, playerStateMachine, weaponCore));

        if (playerCore.currentStamina < playerCore.maxStamina)
        {
            playerCore.currentStamina += playerCore.staminaRegenRate * playerCore.staminaRegenRateMuliplier * playerCore.idleStaminaRegenMultiplier * Time.deltaTime;
        }

        camera.RotationManager(input.LookInput);
        camera.PlayerRotManager(movement.RefinedMovementDirection);

        HandleAttackInput();
    }
    private void HandleAttackInput()
    {
        if (input.LightAttackPressed()) StartAttackChain(attackChainType.Light, weaponCore.CurrentWeaponData.lightAttacks);

        if (input.HeavyAttackPressed()) StartAttackChain(attackChainType.Heavy, weaponCore.CurrentWeaponData.heavyAttacks);

        if (input.SpecialAttackPressed()) StartAttackChain(attackChainType.Special, weaponCore.CurrentWeaponData.specialAttacks);
    }

    private void StartAttackChain(attackChainType type, WeaponAttack[] list)
    {
        if (list == null || list.Length == 0) return;
        WeaponAttack firstAttack = list[0];
        if (playerCore.currentStamina < firstAttack.staminaCost) return;
        weaponCore.attackChainIndex = 0;
        weaponCore.currentChainAttackType = type;
        weaponCore.QueueAttack(firstAttack, type, 0);
        playerStateMachine.SwitchState(new AttackState(playerCore, movement, input, camera, playerStateMachine, weaponCore));

    }
}
