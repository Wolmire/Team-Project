using UnityEngine;

[CreateAssetMenu(fileName = "WeaponAttack", menuName = "Weapons/WeaponAttack")]
public class WeaponAttack : ScriptableObject
{
    public float AttackUptime;
    public float AttackDuration;
    public int baseDamage;
    public float staminaCost;
    public AnimationClip attackAnimation;

   // public void Attack(//AnimatorOverrideController controller, string stateName, Animator animator)
    public void Attack()
    {
        Debug.Log(this.name + " attacked for " + baseDamage + " damage.");
       // controller[stateName] = attackAnimation;
       // animator.SetTrigger("Attack");
    }
}
