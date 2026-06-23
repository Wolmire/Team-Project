using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 1.0f;
    [HideInInspector] public Vector3 RawMovementDirection;
    [HideInInspector] public Vector3 RefinedMovementDirection;   

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

}
