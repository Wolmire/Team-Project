using Unity.VisualScripting;
using UnityEngine;

public class RunState : PlayerState
{
    public RunState(PlayerCore playerCore, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine psm, WeaponStateMachine wsm, WeaponCore weaponCore) : base(playerCore, movement, input, camera, psm, wsm, weaponCore) {}
    public override void Tick()
    {
        if (!input.Sprint) playerStateMachine.SwitchState(new WalkState(playerCore, movement, input, camera, playerStateMachine, weaponStateMachine, weaponCore));
        if (input.MoveInput.sqrMagnitude < 0.01f) playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, camera, playerStateMachine, weaponStateMachine, weaponCore));

        movement.Move(input.MoveInput, camera.Locked, camera.Camera);
        playerCore.MController.Move(movement.RefinedMovementDirection.normalized * movement.RunSpeed * Time.deltaTime);

        camera.RotationManager(input.LookInput);
        camera.PlayerRotManager(movement.RefinedMovementDirection);
        
    }
}
