using UnityEngine;
public class AttackState : PlayerState
{
    public AttackState(PlayerCore playerCore, PlayerMovement movement, PlayerInputManager input, PlayerStateMachine psm, WeaponCore weaponCore, TargetLockHandler targetLock, PlayerEquipManager equipManager) : base(playerCore, movement, input, psm, weaponCore, targetLock, equipManager) { }

    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);

        if (weaponCore.queuedAttack == null) //should never happen but if somehow you enter this state without a queued attack itll just return to idle state for safety
        {
            Debug.LogWarning("No attack queued, returning to idle");
            playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
        }

        weaponCore.OnAttackStarted += ConsumeStamina;//subscribe to attack started event

        weaponCore.StartAttack();
    }

    public override void Tick()
    {
        if (weaponCore.attackFinished)
        {
            if (input.MoveInput.sqrMagnitude > 0.01f) playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));

            else playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
        }

        HandleAttackInput(); //made this a seperate method, makes it easier to copy paste into other states, and is clean
    }
    private void HandleAttackInput()
    {
        if (input.LightAttackPressed()) TryQueueAttack(attackChainType.Light, weaponCore.CurrentWeaponData.lightAttacks);

        if (input.HeavyAttackPressed()) TryQueueAttack(attackChainType.Heavy, weaponCore.CurrentWeaponData.heavyAttacks);

        if (input.SpecialAttackPressed()) TryQueueAttack(attackChainType.Special, weaponCore.CurrentWeaponData.specialAttacks);
    }

    private void TryQueueAttack(attackChainType type, WeaponAttack[] list) //basically checks if the attack the player tries to queue is valid and if it is, queues it up for the next attack in the chain, if not, it does nothing
    {
        if (list == null || list.Length == 0) return;
        if (weaponCore.currentChainAttackType != attackChainType.None && weaponCore.currentChainAttackType != type) return;
        int nextIndex = weaponCore.attackChainIndex + 1;
        if (nextIndex >= list.Length) return;
        var attack = list[nextIndex];
        if (playerCore.currentStamina < attack.staminaCost) return;
        weaponCore.QueueAttack(attack, type, nextIndex);
    }


    //had to make this an event thats sent from weapon core during the attack loop if
    //I wanted weapon core to handle everything instead of having the attack logic in the attack state
    private void ConsumeStamina(WeaponAttack attack) => playerCore.currentStamina -= attack.staminaCost;
    public override void Exit()
    {
        weaponCore.OnAttackStarted -= ConsumeStamina; //unsubscribe from attack started event to avoid memory leaks and multiple subscriptions
    }
}