using UnityEngine;

[CreateAssetMenu(fileName = "WeaponAttack", menuName = "Weapons/WeaponAttack")]
public class WeaponAttack : ScriptableObject
{
    public float attackDuration;
    public int baseDamage;
    public AnimationClip attackAnimation;
    public AttackType attackType;
    public void Attack()
    {
        Debug.Log(this.name + " attacked for " + baseDamage + " damage.");
    }
}
public enum AttackType
{
    Light,
    Heavy,
    Special
}
