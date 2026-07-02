using UnityEngine;
using System.Collections;
using System;

public class WeaponCore : MonoBehaviour
{
    public WeaponData CurrentWeaponData; //this is what youll switch with your weapon swapping script, im not attached to how I do it, make any changes you want

    public WeaponAttack queuedAttack;
    public attackChainType queuedChainAttackType;
    public attackChainType currentChainAttackType;

    public int queuedIndex;
    public int attackChainIndex;

    public bool attackFinished;

    private Coroutine attackRoutine;

    public Action<WeaponAttack> OnAttackStarted;

    public bool IsAttacking => attackRoutine != null;

    public void QueueAttack(WeaponAttack attack, attackChainType type, int index) //done within attack state to try queue the next attack in chain
    {
        queuedAttack = attack;
        queuedChainAttackType = type;
        queuedIndex = index;
    }

    public void StartAttack() //done within other states to start the attack loop (they will also call queueattack), if an attack is queued it will start the loop, if not it will do nothing
    {
        if (queuedAttack == null) return;
        if (attackRoutine != null) StopCoroutine(attackRoutine);

        currentChainAttackType = queuedChainAttackType;
        attackChainIndex = queuedIndex;

        attackRoutine = StartCoroutine(AttackLoop(queuedAttack));
    }

    private IEnumerator AttackLoop(WeaponAttack attack)//attack loop, handles timings and automatically repeats if an attack is queued within the uptime window, if not it will finish the attack and reset the chain
    {
        attackFinished = false;

        while (true)
        {
            queuedAttack = null;
            queuedChainAttackType = attackChainType.None;

            OnAttackStarted?.Invoke(attack); //attack started event that attack state uses to consume stamina, we could use it elsewhere too if we want to do something when an attack starts
            attack.Attack();

            yield return new WaitForSeconds(attack.AttackUptime);

            if (queuedAttack != null && queuedChainAttackType == currentChainAttackType)
            {
                attackChainIndex = queuedIndex;
                attack = queuedAttack;
                continue;
            }
            yield return new WaitForSeconds(attack.AttackDuration - attack.AttackUptime);

            break;
        }

        attackFinished = true;
        attackRoutine = null;

        attackChainIndex = 0;
        currentChainAttackType = attackChainType.None;
    }
}
public enum attackChainType //enum so we can track what attack chain were in so we dont allow the player to queue a different attack type in the middle of an attack chain
{
    None,
    Light,
    Heavy,
    Special
}