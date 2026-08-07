using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEquipManager : MonoBehaviour
{
    IWeaponBase[] Weapons;

    public IWeaponBase[] MainHandWeapons;
    public IWeaponBase[] OffHandWeapons;


    [HideInInspector] public IWeaponBase CurrentWeapon;


    [HideInInspector] public IWeaponBase CurrentAltWeapon;

    public PlayerInput InventoryInput;
        
    private string CurrentHeldName;

    private string CurrentAltHeldName;

    public GameObject Inventory;

    public GameObject LSlot;
    public GameObject RSlot;

    public PlayerAnimator AnimationManager;

    public string ToolBoneName;

    private bool TwoHandedEquipped;

    [HideInInspector] public IWeaponBase WeaponControls;

    public Hand HeldHand;


    int MainHandWeaponInt;
    int OffHandWeaponInt;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Equip(MainHandWeapons[0], RSlot);
    }
    
    public enum Hand
    {
        MainHand,
        OffHand
    }

    public void CycleInventory(Hand HeldHand)
    {
        if (HeldHand == Hand.MainHand)
        {
            MainHandWeaponInt++;
            if(MainHandWeapons.Length - 1 < MainHandWeaponInt)
            {
                MainHandWeaponInt = 0;
            }
            Equip(MainHandWeapons[MainHandWeaponInt], RSlot);
        }
        else
        {
            OffHandWeaponInt++;
            if (OffHandWeapons.Length - 1 < OffHandWeaponInt)
            {
                OffHandWeaponInt = 0;
            }
            Equip(OffHandWeapons[OffHandWeaponInt], LSlot);
        }
    }

    public void Equip(IWeaponBase EquippingWeapon, GameObject Slot)
    {
         Unequip(Slot, EquippingWeapon);

        if (EquippingWeapon.WeaponData.HoldType == EquipType.OneHanded)
        {

            if (Slot == LSlot)
            {
                CurrentAltWeapon = EquippingWeapon;
                CurrentAltWeapon.gameObject.SetActive(true);
                CurrentAltHeldName = CurrentAltWeapon.name;
                CurrentAltWeapon.name = ToolBoneName + ".L";
            }
            else
            {
                CurrentWeapon = EquippingWeapon;
                CurrentWeapon.gameObject.SetActive(true);
                CurrentHeldName = CurrentWeapon.name;
                CurrentWeapon.name = ToolBoneName + ".R";
            }    
            EquippingWeapon.transform.SetParent(Slot.transform);
            TwoHandedEquipped = false;

            //OH NO WHAT TO DO HERE?
        }
        if (EquippingWeapon.WeaponData.HoldType == EquipType.TwoHanded)
        {
            TwoHandedEquipped = true;
            CurrentWeapon = EquippingWeapon;
            CurrentWeapon.gameObject.SetActive(true);
            CurrentHeldName = CurrentWeapon.name;
            CurrentWeapon.name = ToolBoneName + ".R";
            CurrentWeapon.transform.SetParent(RSlot.transform);
        }

        WeaponControls = CurrentWeapon.gameObject.GetComponent<IWeaponBase>();
        AnimationManager.OverrideAnimControl(WeaponControls.WeaponData.AnimOverride);
        AnimationManager.AnimationRebind(); 
    }

    public void Unequip(GameObject ReplacingSlot, IWeaponBase EquippingWeapon)
    {
        if (EquippingWeapon.WeaponData.HoldType != EquipType.OneHanded)
        {
            UnequipBoth();

        }
        else
        {
            if (TwoHandedEquipped)
            {
                if (CurrentWeapon.WeaponData.HoldType == EquipType.Dual)
                {
                    Destroy(CurrentAltWeapon);

                    ObjectUnEquip(CurrentWeapon, CurrentHeldName);
                }
                else
                {
                    UnequipBoth();
                }
            }
            else
            {
                if (ReplacingSlot == LSlot)
                {
                    if (CurrentAltWeapon != null)
                    {
                        ObjectUnEquip(CurrentAltWeapon, CurrentAltHeldName);
                    }
                }
                else
                {
                    if (CurrentWeapon != null)
                    {
                        ObjectUnEquip(CurrentWeapon, CurrentHeldName);
                    }
                }
            }
        }
    }

    public void UnequipBoth()
    {
        if (CurrentWeapon)
        {
            ObjectUnEquip(CurrentWeapon, CurrentHeldName);
        }
        if (CurrentAltWeapon)
        {
            ObjectUnEquip(CurrentAltWeapon, CurrentAltHeldName);
        }
    }
    public void ObjectUnEquip(IWeaponBase Object, string GivingName)
    {
        Object.name = GivingName;
        Object.transform.SetParent(Inventory.transform);
        Object.gameObject.SetActive(false);
    }
}