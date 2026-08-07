using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponCore : MonoBehaviour
{
    public WeaponData CurrentWeaponData;
    public WeaponData CurrentAltWeaponData;//this is what youll switch with your weapon swapping script, im not attached to how I do it, make any changes you want

    [HideInInspector] public WeaponAttack queuedAttack;
    [HideInInspector] public attackChainType queuedChainAttackType;
    [HideInInspector] public attackChainType currentChainAttackType;
    private string AttackTypeString;
    [HideInInspector] public int queuedIndex;
    [HideInInspector] public int attackChainIndex;

    [HideInInspector] public bool attackFinished;

    private Coroutine attackRoutine;

    public WeaponAttack[] MagicSpells;

    [HideInInspector] public Action<WeaponAttack> OnAttackStarted;
    [HideInInspector] public int CurrentSpell;

    public PlayerAnimator PlayerAnim;

   // Animator animator;
    AnimatorOverrideController overrideController;
    public bool IsAttacking => attackRoutine != null;

    public void Start()
    {
        //PlayerAnim = GetComponent<PlayerAnimator>();
       // PlayerAnim.OverrideAnimControl(overrideController);
        //overrideController = new AnimatorOverrideController();
        //overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
        //animator.runtimeAnimatorController = overrideController;

        //runtimeOverride = new AnimatorOverrideController()
        if(PlayerAnim == null)
        {
            Debug.LogWarning("WARNING");
        }
    }
    public void QueueAttack(WeaponAttack attack, attackChainType type, int index) //done within attack state to try queue the next attack in chain
    {
        queuedAttack = attack;
        queuedChainAttackType = type;
        queuedIndex = index;
    }

    public void CycleMagic()
    {
        if(MagicSpells.Length > 1)
        {
            CurrentSpell++;
            if (MagicSpells.Length - 1 < CurrentSpell)
            {
                CurrentSpell = 0;
            }
            ReadySpell();
        }
    }
    private void ReadySpell()
    {
       if(MagicSpells[CurrentSpell].Unique)
       {
           // runtime MagicSpells[CurrentSpell].UniqueAnimation;
       }
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

            //OnAttackStarted?.Invoke(attack); //attack started event that attack state uses to consume stamina, we could use it elsewhere too if we want to do something when an attack starts

            if(queuedChainAttackType == attackChainType.Light)
            {
                AttackTypeString = "Light";
            }
            if (queuedChainAttackType == attackChainType.Heavy)
            {
                AttackTypeString = "Heavy";
            }
            if(queuedChainAttackType == attackChainType.Special)
            {
                AttackTypeString = "Special";
            }
            queuedChainAttackType = attackChainType.None;
            attack.InitiateAttack();
            PlayerAnim.SetAnimTrigger(AttackTypeString);
            //PlayerAnim.Attack(overrideController, "Attack" ,attack.attackAnimation);
            //PlayerAnim.SetAnimTrigger("");

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
    public void OnAttackEvent()
    {
        queuedAttack.InitiateAttack();
    }
}



public enum attackChainType //enum so we can track what attack chain were in so we dont allow the player to queue a different attack type in the middle of an attack chain
{
    None,
    Light,
    Heavy,
    Special
}