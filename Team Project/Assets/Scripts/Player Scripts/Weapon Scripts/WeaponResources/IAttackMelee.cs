using UnityEngine;

public class IAttackMelee : IAttackMechanics
{
    public override void OnInitiateIAttack()
    {
        WeaponHitbox = gameObject;
        Invoke("OnStartIAttack", 3f);
    }

    public override void OnStartIAttack()
    {
        Debug.Log("ATTACKED from" + transform.name);
        WeaponHitbox.SetActive(true);
        Attacking = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (Attacking)
        {
            // collision.gameObject.GetComponent<Health>();
            float Damage = this.gameObject.GetComponentInParent<WeaponData>().AttackDamageModifier;
            Debug.Log("Did " + Damage + " to " + collision.transform.name);
        }
    }
  
    public override void OnEndIAttack()
    {
        WeaponHitbox?.SetActive(false);
        Attacking = false;

    }

}
