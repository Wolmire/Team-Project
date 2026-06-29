using UnityEngine;

public class WeaponIdleState : WeaponState
{
    public WeaponIdleState(PlayerInput input, WeaponCore weaponCore, WeaponStateMachine wsm, PlayerStateMachine psm) : base(input, weaponCore, wsm, psm) {}

    public override void Enter()
    {
        Debug.Log("Entered" + WeaponStateMachine.CurrentState);
    }

    public override void Tick()
    {
        // if(weaponCore.CanQueueAttack && input.Attack)
        // {
        //     WeaponAttack attackToQueue = weaponCore.currentWeapon.lightAttack[0];
        //     weaponCore.QueueAttack(attackToQueue);
        // }

        // if(weaponCore.QueuedAttacks.Count > 0 && weaponCore.CanAttack)
        // {
        //     WeaponStateMachine.SwitchState(new WeaponAttackState(input, weaponCore, WeaponStateMachine, playerStateMachine));
        // }
    }
}
