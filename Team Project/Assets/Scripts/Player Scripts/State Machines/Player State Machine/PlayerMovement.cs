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
    public void Move(Vector2 Direction, bool Locked, GameObject camera, Transform targetPosition)
    {
        RawMovementDirection = new Vector3(Direction.x, 0, Direction.y);

        if (Locked && targetPosition != null)
        {
            // 1. First, face the enemy perfectly on the Y-axis
            FaceLockedOnTarget(targetPosition);

            // 2. Get the clean horizontal direction vector pointing at the enemy
            Vector3 headingToEnemy = targetPosition.position - transform.position;
            headingToEnemy.y = 0;
            headingToEnemy.Normalize();

            // 3. Create a perfect 90-degree right vector based on the enemy heading
            Vector3 rightRelativeVector = Vector3.Cross(Vector3.up, headingToEnemy);

            // 4. Calculate movement: Z input goes toward/away from enemy, X input orbits around enemy
            Vector3 targetMovement = (headingToEnemy * RawMovementDirection.z) + (rightRelativeVector * RawMovementDirection.x);

            // 5. Smoothly apply the direction change
            RefinedMovementDirection = Vector3.Lerp(RefinedMovementDirection, targetMovement, SmoothSpeed * Time.deltaTime);

            // 6. Smooth the X and Y inputs directly for your strafe animator blend tree
            //AnimatorDirection = Vector2.Lerp(AnimatorDirection, Direction, SmoothSpeed * Time.deltaTime);
        }
        else
        {
            // Free movement mode (Relative to camera)
            Vector3 cameraForward = camera.transform.forward;
            Vector3 cameraRight = camera.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 targetMovement = (cameraForward * RawMovementDirection.z) + (cameraRight * RawMovementDirection.x);
            RefinedMovementDirection = Vector3.Lerp(RefinedMovementDirection, targetMovement, SmoothSpeed * Time.deltaTime);

            if (RawMovementDirection.sqrMagnitude > 0.001f)
            {
                Quaternion freeRotation = Quaternion.LookRotation(targetMovement);
                transform.rotation = Quaternion.Slerp(transform.rotation, freeRotation, 15 * Time.deltaTime);
            }

            Vector2 targetFreeDir = new Vector2(0f, Direction.magnitude);
            //AnimatorDirection = Vector2.Lerp(AnimatorDirection, targetFreeDir, SmoothSpeed * Time.deltaTime);
        }

        RefinedMovementDirection.y = 0;
    }
    private void FaceLockedOnTarget(Transform targetPosition)
    {
        // SAFETY CHECK: Prevents errors if the target is suddenly destroyed or lost
        if (targetPosition == null) return;

        // Calculate direction to target
        Vector3 lookDirection = targetPosition.position - transform.position;

        // Keep upright
        lookDirection.y = 0;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15 * Time.deltaTime);
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
