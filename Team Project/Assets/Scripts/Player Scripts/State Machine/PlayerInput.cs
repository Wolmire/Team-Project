using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public Vector2 MoveInput;
    [HideInInspector] public Vector2 LookInput;

    [HideInInspector] public bool IsJumpInputPressed;
    [HideInInspector] public bool Jump = false;

    [HideInInspector] public bool IsSprintInputPressed;
    [HideInInspector] public bool Sprint = false;


    [HideInInspector] public bool IsCrouchInputPressed;
    [HideInInspector] public bool Crouch = false;
    public bool ToggleCrouch;

    public void ToggleBool(ref bool boolToToggle)
    {
        boolToToggle = !boolToToggle;
    }
    public void ReadMoveInput(InputAction.CallbackContext context) => MoveInput = context.ReadValue<Vector2>();

    public void ReadLookInput(InputAction.CallbackContext context) => LookInput = context.ReadValue<Vector2>();

    public void ReadCrouchInput(InputAction.CallbackContext context)
    {
        if (ToggleCrouch)
        {
            if (context.started)
            {
                IsCrouchInputPressed = true;
                ToggleBool(ref Crouch);
            }
            if (context.canceled) IsCrouchInputPressed = false;
        }
        else
        {
           if (context.started)
           {
                IsCrouchInputPressed = true;
                Crouch = true;
           }
           if (context.canceled)
           {
               IsCrouchInputPressed = false;
               Crouch = false;
           }
        }
    }
    public void ReadSprintInput(InputAction.CallbackContext context) =>  Sprint = context.ReadValueAsButton();

    //{
      //  Sprint = context.ReadValueAsButton();

        //if (context.started)
        //{
        //    IsSprintInputPressed = true;
        //    Sprint = true;
        //}
        //if (context.canceled)
        //{
        //    IsSprintInputPressed = false;
        //    Sprint = false;
        //}
    //}
    public void ReadJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsJumpInputPressed = true;
            ToggleBool(ref Jump);
        }
        if (context.canceled) IsJumpInputPressed = false;
    }
}
