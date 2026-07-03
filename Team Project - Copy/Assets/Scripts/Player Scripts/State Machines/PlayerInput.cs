using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public Vector2 MoveInput;
    [HideInInspector] public Vector2 LookInput;

    [HideInInspector] public bool Jump = false;
    [HideInInspector] public bool Sprint = false;
    [HideInInspector] public bool Crouch = false;
    [HideInInspector] public bool Attack = false;

    public bool ToggleCrouch;

    public void ToggleBool(ref bool boolToToggle)
    {
        boolToToggle = !boolToToggle;
    }
    public void ReadMoveInput(InputAction.CallbackContext context) => MoveInput = context.ReadValue<Vector2>();

    public void ReadLookInput(InputAction.CallbackContext context) => LookInput = context.ReadValue<Vector2>();

    public void ReadCrouchInput(InputAction.CallbackContext context)
    {
        if (!ToggleCrouch) Crouch = context.ReadValueAsButton();
        else ToggleBool(ref Crouch);        
    }
    

    public void ReadSprintInput(InputAction.CallbackContext context) =>  Sprint = context.ReadValueAsButton();

    public void ReadJumpInput(InputAction.CallbackContext context) =>  Jump = context.ReadValueAsButton();

    public void ReadAttackInput(InputAction.CallbackContext context) =>  Attack = context.ReadValueAsButton();

}
