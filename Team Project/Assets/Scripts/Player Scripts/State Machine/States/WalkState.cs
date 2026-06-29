using UnityEngine;

public class WalkState : PlayerState
{
    public WalkState(PlayerCore core, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine sm) : base(core, movement, input, camera, sm) { }
    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
        movement.AnimationTriggerer("Walking");
    }
    public override void Tick()
    {
        if (input.MoveInput.sqrMagnitude < 0.01f) playerStateMachine.SwitchState(new IdleState(core, movement, input, camera, playerStateMachine));
        if (input.Sprint) playerStateMachine.SwitchState(new RunState(core, movement, input, camera, playerStateMachine));

        movement.Move(input.MoveInput, camera.Locked, camera.Camera);
        core.MController.Move(movement.RefinedMovementDirection.normalized * movement.WalkSpeed * Time.deltaTime);

        camera.RotationManager(input.LookInput);
        camera.PlayerRotManager(movement.RefinedMovementDirection);
    }
}
