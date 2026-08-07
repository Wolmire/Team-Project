using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponAttack", menuName = "Weapons/WeaponAttack")]
public class WeaponAttack : ScriptableObject
{

    //? Idk if we need this
    public float AttackUptime;
    public float AttackDuration;
    //


    public int baseDamage;
    public float staminaCost;

    GameObject[] HitBox; 
    ParticleSystem[] Effects;
    

    
    public attackType AttackType;
    public magicType MagicType;
    public bool Unique = false;

    public AnimationClip UniqueAnimation;

    public IAttackMechanics IAttack;

    // public void Attack(//AnimatorOverrideController controller, string stateName, Animator animator)
    public void InitiateAttack()
    {
        IAttack.OnInitiateIAttack();
        Debug.Log(this.name + " attacked for " + baseDamage + " damage.");
       // controller[stateName] = attackAnimation;
       // animator.SetTrigger("Attack");
    }
    public void Attack()
    {
        Debug.Log("Attacked, Must have animations");
    }

    public enum attackType
    {
        Melee,
        Magic,
        Ranged
    }

    public enum magicType
    {
        None,
        Cast,
        Beam,
        Summon,
        Effect
    }

}
