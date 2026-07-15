using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private bool IsWalkingCapture;
    public Animator PlayerAnimController;
    public AnimatorStateInfo info;
    float time;
    int CurrentAnimation;
    public RuntimeAnimatorController DefaultAnims;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
       // DefaultAnims = GetComponent<RuntimeAnimatorController>();
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
}
