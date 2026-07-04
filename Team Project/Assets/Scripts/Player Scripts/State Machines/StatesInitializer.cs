using UnityEngine;
public class StatesInitializer : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine;
    public PlayerCore playerCore;
    public WeaponCore weaponCore;
    public PlayerMovement movement;
    public PlayerInputManager input;
    public TargetLockHandler targetLock;
    public PlayerEquipManager equipManager;

    private void Start()
    {
        playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
    }
}