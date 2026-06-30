using UnityEngine;
using System.Collections;

public class WeaponAttackState : WeaponState
{
    public WeaponAttackState(PlayerInput input, WeaponCore weaponCore, WeaponStateMachine wsm, PlayerStateMachine psm) : base(input, weaponCore, wsm, psm) {}

    public override void Enter()
    {
        Debug.Log("Entered" + WeaponStateMachine.CurrentState);
        weaponCore.attackDuration = weaponCore.QueuedAttacks[0].attackDuration;
        weaponCore.StartCoroutine(weaponCore.Attack());
    }
    public override void Tick()
    {        
        if(input.Attack)
        {
            WeaponAttack attackToQueue = weaponCore.currentWeapon.lightAttacks[weaponCore.positionInAttackCombo];
            weaponCore.QueueAttack(attackToQueue);
        }

        if(weaponCore.isAttackingComplete)
        {
            if(weaponCore.QueuedAttacks.Count > 0 && weaponCore.CanAttack)
            {
                WeaponStateMachine.SwitchState(new WeaponAttackState(input, weaponCore, WeaponStateMachine, playerStateMachine));

                if(weaponCore.previousAttack.attackType == weaponCore.currentAttack.attackType && weaponCore.positionInAttackCombo < weaponCore.currentWeapon.lightAttacks.Length - 1)
                {
                    weaponCore.positionInAttackCombo++;
                }
                else
                {
                    weaponCore.positionInAttackCombo = 0;
                }
            }
            else
            {
                WeaponStateMachine.SwitchState(new WeaponIdleState(input, weaponCore, WeaponStateMachine, playerStateMachine));
            }
        }
    }
    public override void Exit()
    {
        weaponCore.IsAttacking = false;
        weaponCore.isAttackingComplete = false;
    }
}
