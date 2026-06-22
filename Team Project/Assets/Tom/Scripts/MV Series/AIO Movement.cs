using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class AIOMovement : MonoBehaviour
{

    public PlayerInput PlayerInputObject;
    CharacterController MController;


    Vector2 RawMovingInput;
    Vector2 RawMouseInput;
    Vector3 RawVelocity;

    Vector3 lastPosition;

    public float Speed = 1.0f;
    Vector3 RawMovementDirection;
    Vector3 RefinedMovementDirection;

    public GameObject Camera;
    float xRotation;
    float yRotation;

    [Header("Limits")]
    public float NegativeXLimit;
    public float PositiveXlimit;

    [Header("Sensitivity")]
    public float XSensitivity = 1;
    public float YSensitivity = 1;

    Vector3 GravityPull;
    public float GravityStrength;


    public bool Locked = false;
    void Start()
    {
        MController = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        InputManager();
        Gravity();

    }

  
    void InputManager()
    {
        RawMovingInput = PlayerInputObject.actions["Move"].ReadValue<Vector2>();
        RawMouseInput = PlayerInputObject.actions["Look"].ReadValue<Vector2>();
        RawVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        if (PlayerInputObject.actions["Tertiary"].IsPressed())
        {
           Locked = !Locked;
        }

        
        if(!Locked)
        {
            if (RawVelocity.magnitude > 0.01f)
            {
                PlayerRotManager(RefinedMovementDirection.normalized);
            }
        }
        if(RawMovingInput.magnitude > 0.01)
        {
            Move(RawMovingInput);
        }
        RotationManager(RawMouseInput);


        if (PlayerInputObject.actions["Jump"].IsPressed())
        {
            Jump(10);
        }

      //  MVInput.actions["Jump".]
    }
    public void RotationManager(Vector2 LookInput)
    {
        xRotation -= LookInput.y * XSensitivity;
        yRotation += LookInput.x * YSensitivity;
        xRotation = Mathf.Clamp(xRotation, NegativeXLimit, PositiveXlimit);

        if (Locked)
        {
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
            Camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);


        }
        else
        {
            //transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            Camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        }
    }
    public void PlayerRotManager(Vector3 PlayerVelocity)
    {
        if (!Locked)
        {
            Vector3 RefinedPlayerVelocity = new Vector3(PlayerVelocity.x, 0, PlayerVelocity.z);
            transform.rotation = Quaternion.LookRotation(RefinedPlayerVelocity, Vector3.up);
            // Camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }
    public void Move(Vector2 Direction)
    {
        RawMovementDirection = new Vector3(Direction.x, 0, Direction.y);

        if (Locked)
        {

            RefinedMovementDirection = transform.TransformDirection(RawMovementDirection);
        }
        else
        {
            RefinedMovementDirection = Camera.transform.TransformDirection(RawMovementDirection);
            RefinedMovementDirection.y = 0;
        }
        MController.Move(RefinedMovementDirection.normalized * Speed * Time.deltaTime);

    }
    public  void Jump(float force)
    {
        Debug.Log("Jumped");
    }
    public  void Gravity()
    {
        GravityPull.y = -GravityStrength;
        MController.Move(GravityPull * Time.deltaTime);
    }
}
