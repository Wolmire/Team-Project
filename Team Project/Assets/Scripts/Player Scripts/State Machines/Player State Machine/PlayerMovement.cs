using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController MController;
    public float WalkSpeed = 1.0f;
    public float SmoothSpeed = 10f;
    public float RunSpeed = 2.0f;
    float DefaultHeight;
    public float CrouchHeight = 1.2f;
    [HideInInspector] public Vector3 RawMovementDirection;
    [HideInInspector] public Vector3 RefinedMovementDirection;
    [HideInInspector] public Vector3 AnimatorDirection;

     public Animator PlayerAnimator;
     
    public void Awake()
    {
        DefaultHeight = MController.height;
        PlayerAnimator = GetComponent<Animator>();
    }
    public void Move(Vector2 Direction, bool Locked, GameObject camera)
    {
        RawMovementDirection = new Vector3(Direction.x, 0, Direction.y);
        

        if (Locked)
        {
            
            RefinedMovementDirection = Vector3.Lerp(RefinedMovementDirection, transform.TransformDirection(RawMovementDirection), SmoothSpeed * Time.deltaTime);
            AnimatorDirection = transform.InverseTransformDirection(RefinedMovementDirection);


        }
        else
        {
            
            RefinedMovementDirection = Vector3.Lerp(RefinedMovementDirection, camera.transform.TransformDirection(RawMovementDirection), SmoothSpeed * Time.deltaTime);
            RefinedMovementDirection.y = 0;
            
            AnimatorDirection.z = RefinedMovementDirection.normalized.magnitude;
            AnimatorDirection.x = 0;
        }
    }
    void Update()
    {
    
        PlayerAnimator.SetFloat("X", AnimatorDirection.x);
        PlayerAnimator.SetFloat("Y", AnimatorDirection.z);
    }

    public void AnimationTriggerer(string TriggerString)
    {
        PlayerAnimator.SetTrigger(TriggerString);
    }

    public void Jump(float force)
    {
        Debug.Log("Jumped");
    }

    public void Crouch()
    {
        MController.height = CrouchHeight;
        MController.center = new Vector3(0, CrouchHeight / 2, 0);
    }

    public void UnCrouch()
    {
        MController.center = new Vector3(0, DefaultHeight / 2, 0);
        MController.height = DefaultHeight;
    }
}
