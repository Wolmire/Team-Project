using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponType weaponType;
    public Image weaponIcon;
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