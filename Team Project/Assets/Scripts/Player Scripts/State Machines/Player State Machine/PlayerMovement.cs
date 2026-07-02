using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController MController;
    
    public float MovementSpeed = 1.0f;
    public float WalkSpeedMultiplier = 1.0f;
    public float RunSpeedMultiplier = 2.0f;
    public float CrouchSpeedMultiplier = 0.75f;
    public float AirSpeedMultiplier = 1f;
    public float SmoothSpeed = 10f;

    public float baseJumpHeight = 1.5f;
    public float jumpHeightMultiplier = 1.0f;
    float DefaultHeight;
    public float CrouchHeight = 1.2f;
    [HideInInspector] public Vector3 RawMovementDirection;
    [HideInInspector] public Vector3 RefinedMovementDirection;
    [HideInInspector] public float velocity;

    public LayerMask ceilingLayer;

    public float GravityStrength;
    
    public void Awake()
    {
        DefaultHeight = MController.height;
    }

    public void Update()
    {
        Gravity();
    }
    public void Move(Vector2 Direction, bool Locked, GameObject camera)
    {
        RawMovementDirection = new Vector3(Direction.x, 0, Direction.y);

        if (Locked)
        {
            RefinedMovementDirection = transform.TransformDirection(RawMovementDirection);
        }
        else
        {
            RefinedMovementDirection = camera.transform.TransformDirection(RawMovementDirection);
            RawMovementDirection = new Vector3(Direction.x, 0, Direction.y);
        }
        if (Locked)
        {
            RefinedMovementDirection = Vector3.Lerp(RefinedMovementDirection, transform.TransformDirection(RawMovementDirection), SmoothSpeed * Time.deltaTime);
            //AnimatorDirection = transform.InverseTransformDirection(RefinedMovementDirection);
        }
        else
        {            
            RefinedMovementDirection = Vector3.Lerp(RefinedMovementDirection, camera.transform.TransformDirection(RawMovementDirection), SmoothSpeed * Time.deltaTime);
            RefinedMovementDirection.y = 0;
        }
    }

    public void Jump()
    {
        float jumpHeight = baseJumpHeight * jumpHeightMultiplier;
        velocity = Mathf.Sqrt(jumpHeight * 2f * GravityStrength);
    }

    public void ApplyMovement(float speedMultiplier)
    {
        Vector3 movedirection = new Vector3(RefinedMovementDirection.x * MovementSpeed * speedMultiplier, velocity, RefinedMovementDirection.z * MovementSpeed * speedMultiplier);
        MController.Move(movedirection * Time.deltaTime);
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
    public bool CheckifCanUncrouch()
    {
        float radius = MController.radius - 0.05f;

        float distance = DefaultHeight - CrouchHeight;

        if (Physics.SphereCast(MController.transform.position + new Vector3(0, CrouchHeight - radius, 0), radius, Vector3.up, out RaycastHit hitInfo, distance, ceilingLayer)) return false;
        else return true;
    }

    public bool isGrounded() => MController.isGrounded;

    public void Gravity()
    {
        if (velocity < 0.1f && MController.isGrounded) velocity = -1f;
        else velocity -= GravityStrength * Time.deltaTime;
    }
}
