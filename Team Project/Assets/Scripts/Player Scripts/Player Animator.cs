using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private bool IsWalkingCapture;
    public Animator PlayerAnimController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetAnimBool(string AnimString, bool AnimBool)
    {
        PlayerAnimController.SetBool(AnimString ,AnimBool);
    }
    public void AnimationRebind()
    {
        IsWalkingCapture = PlayerAnimController.GetBool("Walking");
        PlayerAnimController.Rebind();
        PlayerAnimController.SetBool("Walking", IsWalkingCapture);
    }
    // Update is called once per frame
    public void SetAnimFloat(string AnimString, float AnimFloat)
    {
     PlayerAnimController.SetFloat(AnimString ,AnimFloat);   
    }

}
