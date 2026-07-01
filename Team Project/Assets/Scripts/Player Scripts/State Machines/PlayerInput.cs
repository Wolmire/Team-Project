using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public Vector2 MoveInput;
    [HideInInspector] public Vector2 LookInput;

    [HideInInspector] public bool Jump;
    [HideInInspector] public bool Sprint;
    [HideInInspector] public bool Crouch;
    [HideInInspector] public bool Block;

    private bool lightAttack;
    private bool heavyAttack;
    private bool specialAttack;

    public bool ToggleCrouch;

    public void ReadMoveInput(InputAction.CallbackContext context) => MoveInput = context.ReadValue<Vector2>();
    public void ReadLookInput(InputAction.CallbackContext context) => LookInput = context.ReadValue<Vector2>();

    public void ReadSprintInput(InputAction.CallbackContext context) => Sprint = context.ReadValueAsButton();

    public void ReadJumpInput(InputAction.CallbackContext context) => Jump = context.ReadValueAsButton();
    public bool JumpPressed()
    {
        if (!Jump) return false;
        Jump = false;
        return true;
    }

    public void ReadCrouchInput(InputAction.CallbackContext context)
    {
        if (!ToggleCrouch) Crouch = context.ReadValueAsButton();

        else if (context.started) Crouch = !Crouch;
    }

    public void ReadLightAttackInput(InputAction.CallbackContext context) { if (context.started) lightAttack = true; }
    public bool LightAttackPressed()
    {
        if (!lightAttack) return false;
        lightAttack = false;
        return true;
    }

    public void ReadHeavyAttackInput(InputAction.CallbackContext context) { if (context.started) heavyAttack = true; }
    public bool HeavyAttackPressed()
    {
        if (!heavyAttack) return false;
        heavyAttack = false;
        return true;
    }

    public void ReadSpecialAttackInput(InputAction.CallbackContext context) { if (context.started) specialAttack = true; }
    public bool SpecialAttackPressed()
    {
        if (!specialAttack) return false;
        specialAttack = false;
        return true;
    }

    public void ReadBlockInput(InputAction.CallbackContext context) => Block = context.ReadValueAsButton();

}
