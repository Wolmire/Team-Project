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
    public float weight = 1f;
    public float SmoothingSpeed = 3;
    public float RootSmoothingSpeed = 6;

    public float RayCastOffset = 0.5f;

    //public AvatarIKGoal


    Vector3 LeftLerpIKPos;
    Vector3 RightLerpIKPos;
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
        Debug.DrawRay(FootPosition + Vector3.up * RayCastOffset, Vector3.down * FootRayLength, Color.red);
       if(Physics.Raycast(FootPosition + Vector3.up * RayCastOffset, Vector3.down, out RaycastHit hit, FootRayLength))
        {

            Debug.Log("Hit: " + hit.point);
         //   weight = Mathf.Lerp(weight, 1, SmoothWeight * Time.deltaTime);
            PlayerAnimController.SetIKPositionWeight(foot, 1);


            //PlayerAnimController.SetIKRotationWeight(foot, 1f);

            //  PlayerAnimController.SetBoneLocalRotation(humanBoneId: HumanBodyBones.LeftFoot, hit.normal);
            //Vector3 FootRotation = new Vector3(hit.normal.x, hit.normal.y, hit.normal.z);


            //Transform FootCustomRotation = PlayerAnimController.GetBoneTransform(HumanBodyBones.Hips);

            PlayerAnimController.SetIKPosition(foot, hit.point + Vector3.up * FootOffset);



            //if (foot == AvatarIKGoal.LeftFoot)
            //{
            //    LeftLerpIKPos = Vector3.Lerp(LeftLerpIKPos, hit.point + Vector3.up * FootOffset, SmoothingSpeed * Time.deltaTime);
            //    PlayerAnimController.SetIKPosition(foot, LeftLerpIKPos);


            //}
            //else
            //{
            //    RightLerpIKPos = Vector3.Lerp(RightLerpIKPos, hit.point + Vector3.up * FootOffset, SmoothingSpeed * Time.deltaTime);
            //    PlayerAnimController.SetIKPosition(foot, RightLerpIKPos);
            //}
            //PlayerAnimController.SetIKPosition(foot, hit.point + Vector3.up * FootOffset);


            //Quaternion FootRotation = Quaternion.LookRotation(FootCustomRotation.rotation.eulerAngles, hit.normal);
            //
            // PlayerAnimController.SetBoneLocalRotation(HumanBodyBones.LeftFoot, FootRotation);


            //PlayerAnimController.SetIKRotation(foot, Quaternion.LookRotation(hit.normal));


           


        }
        else
        { 
          //  weight = 0;
        }
        float LeftFootYPos = PlayerAnimController.GetIKPosition(AvatarIKGoal.LeftFoot).y;
        float RightFootYPos = PlayerAnimController.GetIKPosition(AvatarIKGoal.RightFoot).y;
        float LowerFoot = Mathf.Min(LeftFootYPos, RightFootYPos);

        LowerFootYpos = Mathf.Lerp(LowerFootYpos, LowerFoot, RootSmoothingSpeed * Time.deltaTime);
        PlayerAnimController.bodyPosition = new Vector3(PlayerAnimController.rootPosition.x, LowerFootYpos + RootOffset, PlayerAnimController.rootPosition.z);

    }

}