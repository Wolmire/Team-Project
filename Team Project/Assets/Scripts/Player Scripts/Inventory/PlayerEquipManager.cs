using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEquipManager : MonoBehaviour
{
    DebugWeapon[] Weapons;

    public DebugWeapon WeaponSlot1;
    public DebugWeapon WeaponSlot2;

    [HideInInspector] public DebugWeapon CurrentWeapon;

    public PlayerInput InventoryInput;

    private string CurrentHeldName;

    public GameObject Inventory;

    public GameObject LSlot;
    public GameObject RSlot;

    public PlayerAnimator AnimationManager;

    public string ToolBoneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Weapons = new DebugWeapon[] { WeaponSlot1, WeaponSlot2 };
        Equip(WeaponSlot1, RSlot);
    }

    public void Equip(DebugWeapon EquippingWeapon, GameObject Slot)
    {
        if (CurrentWeapon)
        {
            CurrentWeapon.name = CurrentHeldName;
            CurrentWeapon.transform.SetParent(Inventory.transform);
            CurrentWeapon.gameObject.SetActive(false);
        }
        CurrentWeapon = EquippingWeapon;
        CurrentWeapon.gameObject.SetActive(true);
        CurrentHeldName = CurrentWeapon.name;
        CurrentWeapon.name = ToolBoneName;
        CurrentWeapon.transform.SetParent(Slot.transform);
        AnimationManager.AnimationRebind();
    }
}
