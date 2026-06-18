using UnityEngine;
using UnityEngine.InputSystem;

public class MVMManager : MonoBehaviour
{
    MVMoveBase MVMTravel; // Manages walking, Running, 
    MVVerticalBase MVVJump;
    MVLook MVLRotation;

    public PlayerInput MVInput;

    Vector2 RawMovingInput;
    Vector2 RawMouseInput;
    void Start()
    {
        Collect();
    }
    void Collect()
    {
        MVMTravel = GetComponent<MVMoveBase>();
        MVVJump = GetComponent<MVVerticalBase>();
        MVLRotation = GetComponent<MVLook>();
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

        MVMTravel.Move(RawMovingInput);

        MVLRotation.RotationManager(RawMouseInput);


        if (MVInput.actions["Jump"].IsPressed())
        {
            MVVJump.Jump(10);
        }
    }
}
