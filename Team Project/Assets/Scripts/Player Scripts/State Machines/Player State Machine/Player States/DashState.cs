using UnityEngine;

public class DashState : PlayerState
{
    public DashState(PlayerCore playerCore, PlayerMovement movement, PlayerInputManager input, PlayerStateMachine psm, WeaponCore weaponCore, TargetLockHandler targetLock, PlayerEquipManager equipManager) : base(playerCore, movement, input, psm, weaponCore, targetLock, equipManager) { }

    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
        movement.SetMovementDirection(input.MoveInput, targetLock.activeTarget, targetLock.GetActiveCamera(), targetLock.currentTarget);

        movement.StartCoroutine(movement.Dash());
        playerCore.currentStamina -= playerCore.dashStaminaCost;
    }
    public override void Tick()
    {
        if(!movement.isDashing && input.MoveInput.sqrMagnitude > 0.01f) playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
        if (!movement.isDashing && input.MoveInput.sqrMagnitude < 0.01f) playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
    }
}
