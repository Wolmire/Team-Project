using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponCore : MonoBehaviour
{
    public WeaponData currentWeapon;
    public bool CanAttack = false;
    public bool CanQueueAttack = false;
    public bool IsAttacking = false;
    public bool isAttackingComplete = false;
    public float RemoveOldQueuedAttackTime = 0.5f;
    public int positionInAttackCombo = 0;
    public WeaponAttack currentAttack;
    public WeaponAttack previousAttack;
    public float attackDuration;

    public List<WeaponAttack> QueuedAttacks = new List<WeaponAttack>();

    public void QueueAttack(WeaponAttack attack)
    {
        QueuedAttacks.Add(attack);
        StartCoroutine(RemoveOldQueuedAttack(attack));
    }
    private IEnumerator RemoveOldQueuedAttack(WeaponAttack attack)
    {
        yield return new WaitForSeconds(RemoveOldQueuedAttackTime);
        if(QueuedAttacks.Contains(attack)) QueuedAttacks.Remove(attack);
    }

    public IEnumerator Attack()
    {
        IsAttacking = true;
        currentAttack = QueuedAttacks[0];
        currentAttack.Attack();
        QueuedAttacks.RemoveAt(0);
        yield return new WaitForSeconds(attackDuration);
        previousAttack = currentAttack;
        currentAttack = null;
        isAttackingComplete = true;
    }    
}
