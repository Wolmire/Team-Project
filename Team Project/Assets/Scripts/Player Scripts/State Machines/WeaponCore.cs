using UnityEngine;
using System.Collections;
using System;

public class WeaponCore : MonoBehaviour
{
    public WeaponData CurrentWeaponData;

    public WeaponAttack queuedAttack;
    public attackChainType queuedChainAttackType;
    public attackChainType currentChainAttackType;

    public int queuedIndex;
    public int attackChainIndex;

    public bool attackFinished;

    private Coroutine attackRoutine;

    public Action<WeaponAttack> OnAttackStarted;

    public bool IsAttacking => attackRoutine != null;

    public void QueueAttack(WeaponAttack attack, attackChainType type, int index)
    {
        queuedAttack = attack;
        queuedChainAttackType = type;
        queuedIndex = index;
    }

    public void StartAttack()
    {
        if (queuedAttack == null) return;
        if (attackRoutine != null) StopCoroutine(attackRoutine);

        currentChainAttackType = queuedChainAttackType;
        attackChainIndex = queuedIndex;

        attackRoutine = StartCoroutine(AttackLoop(queuedAttack));
    }

    private IEnumerator AttackLoop(WeaponAttack attack)
    {
        attackFinished = false;

        while (true)
        {
            queuedAttack = null;
            queuedChainAttackType = attackChainType.None;

            OnAttackStarted?.Invoke(attack);
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
public enum attackChainType
{
    None,
    Light,
    Heavy,
    Special
}