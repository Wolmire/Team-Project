using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [HideInInspector] public Vector2 MoveInput;
    [HideInInspector] public Vector2 LookInput;

    [HideInInspector] public bool Jump;
    [HideInInspector] public bool Sprint;
    [HideInInspector] public bool Crouch;
    [HideInInspector] public bool Block;
    [HideInInspector] public bool Dash;

    private bool lightAttack;
    private bool heavyAttack;
    private bool specialAttack;

    private bool toggleTargetLock;

    private bool primary;
    private bool secondary;

    public bool ToggleCrouch;

    public string DeviceType;
    public void CheckDeviceType(PlayerInput input)
    {
        DeviceType = input.currentControlScheme;
    }
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
    public void ReadDashInput(InputAction.CallbackContext context) { if (context.started) Dash = true; }
    public bool DashPressed()
    {
        if (!Dash) return false;
        Dash = false;
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

    public void ReadTargetLockInput(InputAction.CallbackContext context) { if (context.started) toggleTargetLock = true; }
    public bool TargetLockPressed()
    {
        if (!toggleTargetLock) return false;
        toggleTargetLock = false;
        return true;
    }
    public void ReadPrimaryInput(InputAction.CallbackContext context) { if (context.started) primary = true; }
    public bool PrimaryInputPressed()
    {
        if (!primary) return false;
        primary = false;
        return true;
    }
    public void ReadSecondaryInput(InputAction.CallbackContext context) { if (context.started) secondary = true; }
    public bool SecondaryInputPressed()
    {
        if (!secondary) return false;
        secondary = false;
        return true;
    }
}
