using UnityEngine;

public abstract class IAttackMechanics : MonoBehaviour
{
    [HideInInspector] public bool Attacking = false;
    [HideInInspector] public GameObject WeaponHitbox;
    private void Start()
    {
        
    }

    public virtual void OnInitiateIAttack()
    {
        Debug.Log("AttackInitiated, Attack will not start unless animations are in place");
    }

    public virtual void OnStartIAttack()
    {

    }
    public virtual void OnEndIAttack()
    {

    }
}
