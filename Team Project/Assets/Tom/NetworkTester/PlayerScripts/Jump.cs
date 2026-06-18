using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
public class Jump : NetworkBehaviour
{
    Rigidbody RB;
    public int jumpForce = 10;
    bool grounded;
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask GroundMask;
    PlayerInput PlayerInputs;
    public override void OnNetworkSpawn()
    {
        PlayerInputs = GetComponent<PlayerInput>();
        RB = GetComponent<Rigidbody>();
       // playerHeight = playerHeight / 2 + 0.2f;
       if (!IsOwner)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        InputMethod();
        grounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, playerHeight, GroundMask);
        if (grounded)
        {
            Debug.Log("Player Is grounded");
        }
        Color rayColor = grounded ? Color.green : Color.red;

        Debug.DrawRay(transform.position, Vector3.down * playerHeight, rayColor);
    }
    public void InputMethod()
    {
        if (PlayerInputs.actions["Jump"].IsPressed())
        {
            Debug.Log("JumpPressed");
            if (grounded)
            {
                Debug.Log("Jumped");
                JumpMethod();
            }
        }
    }
    public void JumpMethod()
    {
        RB.linearVelocity = new Vector3(RB.linearVelocity.x, 0f, RB.linearVelocity.z);

        RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
