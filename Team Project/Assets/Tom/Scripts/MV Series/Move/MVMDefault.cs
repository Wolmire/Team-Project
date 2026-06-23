using UnityEngine;

public class MVMDefault : B_MVMove
{
    public float MVMDSpeed = 1.0f;
    Vector3 RawMovementDirection;
    Vector3 RefinedMovementDirection;
    CharacterController MVMController;

    public GameObject Camera;

    private void Awake()
    {
        MVMController = GetComponent<CharacterController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Move(Vector2 Direction)
    {
        RawMovementDirection = new Vector3(Direction.x, 0, Direction.y);

        if (Locked)
        {

            RefinedMovementDirection = transform.TransformDirection(RawMovementDirection);
        }
        else
        {
            RefinedMovementDirection = Camera.transform.TransformDirection(RawMovementDirection);
        }
        MVMController.Move(RefinedMovementDirection * MVMDSpeed * Time.deltaTime);

    }
}
