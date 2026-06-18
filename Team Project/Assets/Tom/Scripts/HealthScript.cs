using UnityEngine;

public class HealthScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int health = 100;
    //public int Health
    //{
    //    get { return Health; }
    //    protected set
    //    {
    //        Health = Mathf.Clamp(value, 0, 100);
    //        if (Health <= 0)
    //        {
    //            Die();
    //        }
    //    }
    //}

    private void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }
    public void TakeDamage(int DamageAmount, string DamageType)
    {
        health -= DamageAmount;
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
