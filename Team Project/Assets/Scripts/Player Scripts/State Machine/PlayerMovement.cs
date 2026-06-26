using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController MController;
    public float Speed = 1.0f;
    float DefaultHeight;
    public float CrouchHeight = 1.2f;
    [HideInInspector] public Vector3 RawMovementDirection;
    [HideInInspector] public Vector3 RefinedMovementDirection;   

    public void Awake()
    {
        DefaultHeight = MController.height;
        Time.timeScale = .1f;
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
            RefinedMovementDirection.y = 0;
        }
    }

    public  void Jump(float force)
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
