using UnityEngine;
using UnityEngine.InputSystem;

public class MVMManager : MonoBehaviour
{
    B_MVMove MVMTravel; // Manages walking, Running, 
    B_MVVertical MVVJump;
    B_MVLook MVLRotation;

    public PlayerInput MVInput;

    Vector2 RawMovingInput;
    Vector2 RawMouseInput;
    Vector3 RawVelocity;

    Vector3 lastPosition;

    public bool Locked = false;
    void Start()
    {
        Collect();
    }
    void Collect()
    {
        MVMTravel = GetComponent<B_MVMove>();
        MVVJump = GetComponent<B_MVVertical>();
        MVLRotation = GetComponent<B_MVLook>();
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
        MVVJump.Gravity();
    }

  
    void InputManager()
    {
        RawMovingInput = MVInput.actions["Move"].ReadValue<Vector2>();
        RawMouseInput = MVInput.actions["Look"].ReadValue<Vector2>();
        RawVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        if (MVInput.actions["Tertiary"].IsPressed())
        {
            if(Locked)
            {
                Locked = false;
            }
            else
            {
                Locked = true;
            }
        }

        if (Locked)
        {
            MVLRotation.Locked = true;
            MVMTravel.Locked = true;

        }
        else
        {
            MVLRotation.Locked = false;
            MVMTravel.Locked = false;
            if (RawVelocity.magnitude > 0.01f)
            {
                MVLRotation.PlayerManager(RawVelocity.normalized);
            }
        }
        MVMTravel.Move(RawMovingInput);

        MVLRotation.RotationManager(RawMouseInput);


        if (MVInput.actions["Jump"].IsPressed())
        {
            MVVJump.Jump(10);
        }
    }
}
