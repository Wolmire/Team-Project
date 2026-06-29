using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerCore playerCore, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine psm, WeaponStateMachine wsm, WeaponCore weaponCore) : base(playerCore, movement, input, camera, psm, wsm, weaponCore) { }
    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
        weaponCore.CanQueueAttack = true;
        weaponCore.CanAttack = true;
        movement.AnimationTriggerer("Idle");
    }
    public override void Tick()
    {
        if(input.MoveInput.sqrMagnitude > 0.01f) playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, camera, playerStateMachine, weaponStateMachine, weaponCore));
        if(input.Crouch) playerStateMachine.SwitchState(new CrouchState(playerCore, movement, input, camera, playerStateMachine, weaponStateMachine, weaponCore));

        camera.RotationManager(input.LookInput);
        camera.PlayerRotManager(movement.RefinedMovementDirection);
    }
}
