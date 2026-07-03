using UnityEngine;
public class StatesInitializer : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine;
    public WeaponStateMachine weaponStateMachine;
    public PlayerCore playerCore;
    public WeaponCore weaponCore;
    public PlayerMovement movement;
    public PlayerInput input;
    public PlayerCamera camera;

    private void Start()
    {
        playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, camera, playerStateMachine, weaponStateMachine, weaponCore));
        weaponStateMachine.SwitchState(new WeaponIdleState(input, weaponCore, weaponStateMachine, playerStateMachine));
    }
}