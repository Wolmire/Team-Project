using UnityEngine;
public class AttackState : PlayerState
{
    public AttackState(PlayerCore playerCore,PlayerMovement movement,PlayerInput input,PlayerCamera camera,PlayerStateMachine psm,WeaponCore weaponCore) : base(playerCore, movement, input, camera, psm, weaponCore) { }

    public override void Enter()
    {
        weaponCore.OnAttackStarted += ConsumeStamina;

        if (weaponCore.queuedAttack == null)
        {
            Debug.LogWarning("No attack queued");
            return;
        }
        weaponCore.StartAttack();
    }

    public override void Tick()
    {
        if (!weaponCore.attackFinished) return;

        if (input.MoveInput.sqrMagnitude > 0.01f) playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, camera, playerStateMachine, weaponCore));

        else playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, camera, playerStateMachine, weaponCore));

        camera.RotationManager(input.LookInput);
        camera.PlayerRotManager(movement.RefinedMovementDirection);

        HandleAttackInput();
    }
    private void HandleAttackInput()
    {
        if (input.LightAttackPressed()) TryQueueAttack(attackChainType.Light, weaponCore.CurrentWeaponData.lightAttacks);

        if (input.HeavyAttackPressed()) TryQueueAttack(attackChainType.Heavy, weaponCore.CurrentWeaponData.heavyAttacks);

        if (input.SpecialAttackPressed()) TryQueueAttack(attackChainType.Special, weaponCore.CurrentWeaponData.specialAttacks);
    }

    private void TryQueueAttack(attackChainType type, WeaponAttack[] list)
    {
        if (list == null || list.Length == 0) return;
        if (weaponCore.currentChainAttackType != attackChainType.None && weaponCore.currentChainAttackType != type) return;
        int nextIndex = weaponCore.attackChainIndex + 1;
        if (nextIndex >= list.Length) return;
        var attack = list[nextIndex];
        if (playerCore.currentStamina < attack.staminaCost) return;
        weaponCore.QueueAttack(attack, type, nextIndex);
    }
    private void ConsumeStamina(WeaponAttack attack)
    {
        playerCore.currentStamina -= attack.staminaCost;
    }

    public override void Exit()
    {
        weaponCore.OnAttackStarted -= ConsumeStamina;
    }
}