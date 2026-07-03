using UnityEngine;
using UnityEngine.InputSystem;

public class EquipManager : MonoBehaviour
{
    IWeaponBase[] Weapons;

    public IWeaponBase WeaponSlot1;
    public IWeaponBase WeaponSlot2;

    [HideInInspector] public IWeaponBase CurrentWeapon;

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
        Weapons = new IWeaponBase[] { WeaponSlot1, WeaponSlot2 };
        Equip(WeaponSlot1, RSlot);
    }

    void Equip(IWeaponBase EquippingWeapon, GameObject Slot)
    {
        if (CurrentWeapon)
        {
            CurrentWeapon.name = CurrentHeldName;
            CurrentWeapon.transform.SetParent(Inventory.transform);
        }
        CurrentWeapon = EquippingWeapon;
        CurrentHeldName = CurrentWeapon.name;
        CurrentWeapon.name = ToolBoneName;
        CurrentWeapon.gameObject.transform.SetParent(Slot.transform);
        AnimationManager.AnimationRebind();

    }

    // Update is called once per frame
    void Update()
    {
        //if (InventoryInput.actions[""].WaspressedThisFrame() && CurrentWeapon != WeaponSlot1)
        //{

        //}
        ////if (InventoryInput.actions[""].WaspressedThisFrame() && CurrentWeapon != WeaponSlot2)
        //{

        //}
    }
    public void ReadPrimaryEquipInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Equip(WeaponSlot1, RSlot);
        }
    }
    public void ReadSecondaryEquipInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Equip(WeaponSlot2, RSlot);
        }
    }
}
