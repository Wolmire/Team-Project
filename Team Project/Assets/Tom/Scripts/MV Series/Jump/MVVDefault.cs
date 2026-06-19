using UnityEngine;

public class MVVDefault : B_MVVertical
{

    Vector3 GravityPull;
    public float GravityStrength;

    CharacterController MVVController;

    private void Awake()
    {
        MVVController = GetComponent<CharacterController>();
    }
    public override void Jump(float force)
   {
        Debug.Log("Jumped");
   }

   public override void ClimbA(Vector3 Point)
   {
       
   }

   public override void ClimbO(Vector3 Point)
   {
        
   }
   public override void Gravity()
    {
        GravityPull.y = -GravityStrength;
        MVVController.Move(GravityPull * Time.deltaTime);
    }
}
