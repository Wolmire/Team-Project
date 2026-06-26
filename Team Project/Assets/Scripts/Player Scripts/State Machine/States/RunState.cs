using Unity.VisualScripting;
using UnityEngine;

public class RunState : PlayerState
{
    public RunState(PlayerCore core, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine sm) : base(core, movement, input, camera, sm)
    {
    }
    public override void Tick()
    {
        if (!input.Sprint) playerStateMachine.SwitchState(new WalkState(core, movement, input, camera, playerStateMachine));
        if (input.MoveInput.sqrMagnitude < 0.01f) playerStateMachine.SwitchState(new IdleState(core, movement, input, camera, playerStateMachine)); 

        movement.Move(input.MoveInput, camera.Locked, camera.Camera);
        core.MController.Move(movement.RefinedMovementDirection.normalized * movement.RunSpeed * Time.deltaTime);

        camera.RotationManager(input.LookInput);
        camera.PlayerRotManager(movement.RefinedMovementDirection);
        
    }
}
