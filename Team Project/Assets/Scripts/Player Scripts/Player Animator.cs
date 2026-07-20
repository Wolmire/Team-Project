using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerAnimator : MonoBehaviour
{
    private bool IsWalkingCapture;
    public Animator PlayerAnimController;
    public AnimatorStateInfo info;
    float time;
    int CurrentAnimation;
    public RuntimeAnimatorController DefaultAnims;
    public float FootOffset = 0.17f;

    public float FootRayLength = 0.3f;

    private float LowerFootYpos;
    public float RootOffset = 0.5f;

    public float SmoothingSpeed;
    //public AvatarIKGoal
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
       // DefaultAnims = GetComponent<RuntimeAnimatorController>();
    }
    private void Update()
    {
        //FootIk();
    }
    public void SetAnimBool(string AnimString, bool AnimBool)
    {
        PlayerAnimController.SetBool(AnimString, AnimBool);
    }
    public void AnimationRebind()
    {
        IsWalkingCapture = PlayerAnimController.GetBool("Walking");

        info = PlayerAnimController.GetCurrentAnimatorStateInfo(0);
        time = info.normalizedTime;
        CurrentAnimation = info.fullPathHash;

        var cache = new Dictionary<string, object>();
        foreach (var param in PlayerAnimController.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Float)
            {
                cache[param.name] = PlayerAnimController.GetFloat(param.name);
            }
        }

        //
        PlayerAnimController.Rebind();
        //
        PlayerAnimController.Play(CurrentAnimation, 0, time);
        PlayerAnimController.SetBool("Walking", IsWalkingCapture);

        foreach (var param in PlayerAnimController.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Float)
            {
                PlayerAnimController.SetFloat(param.name, (float)cache[param.name]);
            }
        }
    }
    // Update is called once per frame
    public void SetAnimFloat(string AnimString, float AnimFloat)
    {
        PlayerAnimController.SetFloat(AnimString, AnimFloat);
    }
    public void OverrideAnimControl(AnimatorOverrideController OverrideAnim)
    {
       //OverrideAnim = new AnimatorOverrideController();
      // OverrideAnim.runtimeAnimatorController = PlayerAnimController.runtimeAnimatorController;
        if (OverrideAnim != null)
        {
            PlayerAnimController.runtimeAnimatorController = OverrideAnim;
        }
        else
        {
            PlayerAnimController.runtimeAnimatorController = DefaultAnims;
        }
       // AnimationRebind();
    }

    public void Attack(AnimatorOverrideController OverrideAnim, string StateName,AnimationClip AnimClip)
    {
      
         OverrideAnim[StateName] = AnimClip;
      
    }

    public void SetAnimTrigger(string AnimString)
    {
        PlayerAnimController.SetTrigger(AnimString);
    }

    void OnAnimatorIK(int layerIndex)
    {
        FootIk(AvatarIKGoal.LeftFoot);
        FootIk(AvatarIKGoal.RightFoot);

    }

    void FootIk(AvatarIKGoal foot)
    {
        Vector3 FootPosition = PlayerAnimController.GetIKPosition(foot);
        Debug.DrawRay(FootPosition, Vector3.down * FootRayLength, Color.red);
       if(Physics.Raycast(FootPosition, Vector3.down, out RaycastHit hit, FootRayLength))
        {

            Debug.Log("Hit: " + hit.point);
            PlayerAnimController.SetIKPositionWeight(foot, 1f);
            PlayerAnimController.SetIKPosition(foot, hit.point + Vector3.up * FootOffset);

         //   PlayerAnimController.SetIKRotationWeight(foot, 1f);
          //  PlayerAnimController.SetIKRotation(foot, Quaternion.Euler(hit.normal));

            float LeftFootYPos = PlayerAnimController.GetIKPosition(AvatarIKGoal.LeftFoot).y;
            float RightFootYPos = PlayerAnimController.GetIKPosition(AvatarIKGoal.RightFoot).y;
            float LowerFoot = Mathf.Min(LeftFootYPos, RightFootYPos);

            LowerFootYpos = Mathf.Lerp(LowerFootYpos, LowerFoot, SmoothingSpeed * Time.deltaTime);


            PlayerAnimController.bodyPosition = new Vector3(PlayerAnimController.rootPosition.x, LowerFootYpos + RootOffset, PlayerAnimController.rootPosition.z);
        }
        else
        {
            PlayerAnimController.SetIKPositionWeight(foot, 0f);
        }


    }

}