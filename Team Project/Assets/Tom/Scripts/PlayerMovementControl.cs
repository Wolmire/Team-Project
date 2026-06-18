using DG.Tweening;
using JetBrains.Annotations;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Splines;

public class PlayerMovementControl : MonoBehaviour
{
    public CharacterController CharControl;
    [Header("Speed")]
    float Speed;
    float MaxSpeed;
    public float WalkSpeed;
    public float RunSpeed;

    [Header("Gravity")]
    public float Gravity;
    public float FallGravityMultiplier;
    public float FallMultiplier;
    public float FallAccelDuration;

    [Header("Jumping")]
    public float JumpForce;
    public float JumpDuration;

    Vector3 MovementDirection;
    Vector3 GravityPull;
    //Vector3 

    [Header("Control")]
    public float Sensitivity;
    public PlayerInput playerInput;

    public Transform PlayerCam;
    Animator PlayerAnimator;

    Vector2 MouseMovement;

    float xRotation;
    float yRotation;

    Vector3 lastPosition;
    Vector3 Velocity;
    Vector2 RawMovingInput;
    Vector3 localVelocity;
    float CurrentSpeed;
    float verticalVelocity;

    bool Falling;
    bool Jumping = false;
    float JumpTimer;

    bool FallingStart;

    float t;

    float FallingTime;
    float StepOffsetHolder;


    public float RayForwardOffset;
    public float RayUpwardOffset;
    public float RaycastDist;

    public LayerMask Environment;
    Vector3 ClimbOffset;

    RaycastHit ClimbRayInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StepOffsetHolder = CharControl.stepOffset;
        MaxSpeed = RunSpeed;
        PlayerAnimator = GetComponent<Animator>();
       // Transform Arms = PlayerCam.Find("ArmsOnly");



        //ArmsAnimator = Arms.GetComponent<Animator>();

        if(!PlayerAnimator)
        {
            Debug.LogWarning("NOANIMATOR");
        }
    }

    // Update is called once per frame
    void Update()
    {
  
        // AnimatorManager();
        if (CharControl.enabled)
        {
            MovementInputManager();
            MovementManager();
            RotationManager();
        }

        Velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        if (!CharControl.isGrounded && Velocity.y < 0.1f)
        {
            Falling = true;
            PlayerAnimator.SetTrigger("IsFalling");
        }
        else
        {
            Falling = false;
        }
        //Debug.Log("Velocity: " + Velocity);
    }
    void MovementInputManager()
    {
        //if(t > 1f)
        //{
        //    t = 0.0f;
        //}
        RawMovingInput = playerInput.actions["Move"].ReadValue<Vector2>();
        if (playerInput.actions["Sprint"].IsPressed())
        {
            //  t += Time.deltaTime * 0.3f;
            Speed = RunSpeed;
               // Speed = RunSpeed;
        }
        else
        {
            Speed = WalkSpeed;
           // t += Time.deltaTime * 0.3f;

        }
        MovementDirection = new Vector3(RawMovingInput.x, 0f, RawMovingInput.y);

        //  MovementDirection = new Vector3(Input.GetAxis(""), 0f, Input.GetAxis("Vertical"));

        MouseMovement = playerInput.actions["Look"].ReadValue<Vector2>();

        VerticalManager();
        Debug.DrawRay(transform.position + ClimbOffset, Vector3.down * RaycastDist, Color.red);

        if (playerInput.actions["Jump"].IsPressed())
        {
            float ClimbForwardMultiplier = 1;
            float ClimbUpwardMultiplier = 1;
            ClimbOffset = transform.forward * RayForwardOffset * ClimbForwardMultiplier + transform.up * RayUpwardOffset * ClimbUpwardMultiplier;
            if (CharControl.isGrounded)
            {
                Debug.Log("jumped");
                if(Physics.Raycast(transform.position + ClimbOffset, Vector3.down, out RaycastHit ClimbRayHit , RaycastDist, Environment) && RawMovingInput.y > 0.1f)
                {
                    PlayerAnimator.SetTrigger("IsClimbing");
                    PlayerAnimator.applyRootMotion = true;
                    CharControl.enabled = false;
                    ClimbRayInfo = ClimbRayHit;
                    if(Physics.Raycast(transform.position, transform.forward, out RaycastHit ForClimbRayHit, RaycastDist, Environment))
                    {

                        Vector3 ClimbingPosition = new Vector3(ForClimbRayHit.point.x, ClimbRayInfo.point.y - CharControl.height/4f, ForClimbRayHit.point.z);
                        transform.DOMove(ClimbingPosition + (ForClimbRayHit.normal * CharControl.radius), 2f)
                        //.SetEase(Ease.InOutCubic)
                        .OnComplete(() =>
                        {
                            PlayerAnimator.applyRootMotion = false;
                            PlayerAnimator.SetTrigger("IsClimbing");
                            CharControl.enabled = true;

                            //ClimbingPosition = ClimbRayInfo.point + Vector3.up * (CharControl.height / 2f);
                            //transform.DOMoveY(ClimbingPosition.y, 1f)
                            //.OnComplete(() =>
                            //{
                            //    ClimbingPosition = ClimbRayInfo.point + Vector3.up * (CharControl.height / 2f) + transform.forward * 0.2f;
                            //    transform.DOMove(ClimbingPosition, 1f)
                            //    .OnComplete(() =>
                            //     {
                            //        Debug.Log("FinishedClimbing");
                            //         PlayerAnimator.applyRootMotion = false;
                            //         CharControl.enabled = true;
                            //     });
                            //});
                        });
                    }
                    else
                    {
                        CharControl.enabled = true;
                        Debug.Log("ClimbingBRoke");
                    }
                    

                }
                else
                {
                    PlayerAnimator.SetTrigger("IsJumping");
                    Jumping = true;
                    JumpTimer = 0f;
                }           
            }
            else
            {
                Debug.Log("CANT JUMP UNGROUNDED");
            }
        }
    }

    void VerticalManager()
    {
        if (!Falling)
        {
            // GravityPull.y = Gravity;
            FallingTime = 0;
            FallingStart = true;
        }
        else if (!Jumping)
        {
            if (FallingStart)
            {
                GravityPull.y = 0f;
                FallingStart = false;
                FallingTime = 0;
            }
            FallingTime += Time.deltaTime;


            float t = FallingTime / FallAccelDuration;
            if (GravityPull.y > Gravity * FallGravityMultiplier)
            {
                GravityPull.y -= (t * t) * FallMultiplier * Time.deltaTime;
                // Debug.Log("GoingToMaxFallSpeed " + GravityPull.y + "   Fall Time: " + FallingTime);


            }
            else
            {
                GravityPull.y = Gravity * FallGravityMultiplier;
                Debug.Log("ReachedMaxFallSpeed " + GravityPull.y);
            }
        }

    
        if (Jumping)
        {

            JumpTimer += Time.deltaTime;
            float n = JumpTimer / JumpDuration;
            GravityPull.y = JumpForce * (1 - Mathf.Sqrt(n));
            //            GravityPull.y = JumpForce * (1 - Mathf.Sqrt(JumpTimer / JumpDuration));


            if (JumpTimer >= JumpDuration)
            {
                Jumping = false;
            }

        }
    }

    void AnimatorManager()
    {
        localVelocity = transform.InverseTransformDirection(Velocity);
        PlayerAnimator.SetFloat("SidewaysMovement", localVelocity.x / MaxSpeed);
        PlayerAnimator.SetFloat("ForwardMovement", localVelocity.z / MaxSpeed);

       // PlayerAnimator.speed = RawMovingInput.y;

    }



    void MovementManager()
    {
       

        //float targetSpeed = localVelocity.z / Speed;

        if (CurrentSpeed != Speed)
        {
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, Speed, t);
            t = Time.deltaTime;

        }




        Vector3 MoveVector = transform.TransformDirection(MovementDirection);
        if (CharControl.isGrounded && !Jumping)
        {
            GravityPull.y = -2f;
        }
        if (CharControl.enabled)
        {
            CharControl.Move(MoveVector * CurrentSpeed * Time.deltaTime);
            CharControl.Move(GravityPull * Time.deltaTime);
            AnimatorManager();
        }


    }

    
    void RotationManager()
    {
        xRotation -= MouseMovement.y * Sensitivity;
        yRotation += MouseMovement.x * Sensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 69f); 

        PlayerCam.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

}
