using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    public WeaponType weaponType;    
    public float AttackDamageModifier;
    public float AttackSpeedModifier;
    public WeaponAttack[] lightAttacks;
    public WeaponAttack[] heavyAttacks;
    public WeaponAttack[] specialAttacks;
}
public enum WeaponType
{
    ShortSword,
    LongSword
}