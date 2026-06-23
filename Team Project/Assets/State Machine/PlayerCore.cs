using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCore : MonoBehaviour
{
    [HideInInspector] public Vector2 MoveInput;

    [HideInInspector] public bool IsJumpInputPressed;
    [HideInInspector] public bool Jump = false;

    [HideInInspector] public bool IsSprintInputPressed;
    [HideInInspector] public bool Sprint = false;


    [HideInInspector] public bool IsCrouchInputPressed;
    [HideInInspector] public bool Crouch = false;
    public bool ToggleCrouch;

    [HideInInspector] public bool waitComplete = false;
    public void ToggleBool(ref bool boolToToggle)
    {
        boolToToggle = !boolToToggle;
    }

    public void LeaveStateAfterDelay(float stateDuration)
    {
        waitComplete = false;
        StartCoroutine(LeaveStateAfterDelayCoroutine(stateDuration));
    }
    private IEnumerator LeaveStateAfterDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        waitComplete = true;
    }

    public void ReadMoveInput(InputAction.CallbackContext context) => MoveInput = context.ReadValue<Vector2>();
    public void ReadCrouchInput(InputAction.CallbackContext context)
    {
        if(ToggleCrouch)
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
    public void ReadSprintInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsSprintInputPressed = true;
            Sprint = true;
        }
        if (context.canceled)
        {
            IsSprintInputPressed = false;
            Sprint = false;
        }
    }
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
 