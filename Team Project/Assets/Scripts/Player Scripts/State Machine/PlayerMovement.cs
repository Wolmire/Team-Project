using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
<<<<<<< Updated upstream:Team Project/Assets/Scripts/Player Scripts/State Machine/PlayerMovement.cs
=======
    public CharacterController MController;

    public PlayerAnimator PlayerAnim;

>>>>>>> Stashed changes:Team Project/Assets/Scripts/Player Scripts/State Machines/Player State Machine/PlayerMovement.cs
    public float WalkSpeed = 1.0f;
    public float RunSpeed = 1.4f;
    public float SmoothSpeed = 10f;
    [HideInInspector] public Vector3 RawMovementDirection;
    [HideInInspector] public Vector3 RefinedMovementDirection;
    [HideInInspector] public Vector3 AnimatorDirection;

<<<<<<< Updated upstream:Team Project/Assets/Scripts/Player Scripts/State Machine/PlayerMovement.cs
     public Animator PlayerAnimator;

    private void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
=======
     
    public void Awake()
    {
        DefaultHeight = MController.height;
>>>>>>> Stashed changes:Team Project/Assets/Scripts/Player Scripts/State Machines/Player State Machine/PlayerMovement.cs
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

        PlayerAnim.SetAnimFloat("X", AnimatorDirection.x);
        PlayerAnim.SetAnimFloat("Y", AnimatorDirection.z);
    }

    public void AnimationBool(string SettingBool, bool AnimationBool)
    {
        PlayerAnim.SetAnimBool(SettingBool, AnimationBool);
    }

    public void Jump(float force)
    {
        Debug.Log("Jumped");
    }

}
