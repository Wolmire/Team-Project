using JetBrains.Annotations;
using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerCore playerCore, PlayerMovement movement, PlayerInputManager input, PlayerStateMachine psm, WeaponCore weaponCore, TargetLockHandler targetLock, PlayerEquipManager equipManager) : base(playerCore, movement, input, psm, weaponCore, targetLock, equipManager) { }

    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
        movement.Jump();    
    }

    public override void Tick()
    {
        movement.ApplyMovement(movement.AirSpeedMultiplier);
        if (movement.OnLedge)
        {
            playerStateMachine.SwitchState(new LedgeState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
        }
        else
        {
            if (movement.isGrounded())
            {
                if (movement.MovementSpeed > 0.1)
                {
                    playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));

                }
                else
                {
                    playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));

                }
            }

            if (movement.MController.velocity.y < -0.1f)
            {
                playerStateMachine.SwitchState(new FallState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
            }
        }
    }
}
