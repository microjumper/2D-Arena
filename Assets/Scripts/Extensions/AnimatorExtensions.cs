using UnityEngine;

public static class AnimatorExtensions
{
    public static void SetTrigger(this Animator animator, AnimatorParameter parameter)
    {
        animator.SetTrigger(parameter.ToString());
    }

    public static bool GetBool(this Animator animator, AnimatorParameter parameter)
    {
        return animator.GetBool(parameter.ToString());
    }

    public static void SetBool(this Animator animator, AnimatorParameter parameter, bool value)
    {
        animator.SetBool(parameter.ToString(), value);
    }
}