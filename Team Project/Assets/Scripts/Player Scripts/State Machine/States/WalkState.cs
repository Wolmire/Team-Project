using UnityEngine;

public class WalkState : PlayerState
{
    public WalkState(PlayerCore core, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine sm) : base(core, movement, input, camera, sm) { }
    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
    }
    public override void Tick()
    {
        if (input.MoveInput.sqrMagnitude < 0.01f) playerStateMachine.SwitchState(new IdleState(core, movement, input, camera, playerStateMachine));

        movement.Move(input.MoveInput, camera.Locked, camera.Camera);
        core.MController.Move(movement.RefinedMovementDirection.normalized * movement.Speed * Time.deltaTime);

        camera.RotationManager(input.LookInput);
        camera.PlayerRotManager(movement.RefinedMovementDirection);
    }
}
