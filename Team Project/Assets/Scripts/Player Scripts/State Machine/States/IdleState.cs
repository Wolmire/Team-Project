using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerCore core, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine sm) : base(core, movement, input, camera, sm) { }
    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
    }
    public override void Tick()
    {
        if(input.MoveInput.sqrMagnitude > 0.01f) playerStateMachine.SwitchState(new WalkState(core, movement, input, camera, playerStateMachine));
        camera.RotationManager(input.LookInput);
        camera.PlayerRotManager(movement.RefinedMovementDirection);
    }
}
