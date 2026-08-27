using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class RunState : PlayerState
{
    public RunState(PlayerCore playerCore, PlayerMovement movement, PlayerInputManager input, PlayerStateMachine psm, WeaponCore weaponCore, TargetLockHandler targetLock, PlayerEquipManager equipManager) : base(playerCore, movement, input, psm, weaponCore, targetLock, equipManager) { }
    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
    }

    public override void Tick()
    {
        if (movement.OnLedge) playerStateMachine.SwitchState(new LedgeState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));

        if (!input.Sprint) playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
        if (input.MoveInput.sqrMagnitude < 0.01f) playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
        if (!movement.isGrounded()) playerStateMachine.SwitchState(new FallState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));

        if (input.JumpPressed())
        {
            movement.AirSpeedMultiplier = movement.RunSpeedMultiplier;
            playerStateMachine.SwitchState(new JumpState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
        }


        movement.Move(input.MoveInput, targetLock.activeTarget, targetLock.GetActiveCamera(), targetLock.currentTarget);
        movement.Gravity();
        movement.ApplyMovement(movement.RunSpeedMultiplier);
                
        playerCore.currentStamina -= playerCore.runStaminaCost * Time.deltaTime;

        if (playerCore.currentStamina <= 0)
        {
            playerCore.currentStamina = 0;

            playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
        }
    }

    public override void Exit()
    {
        movement.AirSpeedMultiplier = movement.RunSpeedMultiplier;
    }
}
