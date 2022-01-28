using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineRemote : MonoBehaviour
{
    private SkeletonAnimation _animation;

    private void Awake()
    {
        _animation = GetComponent<SkeletonAnimation>();
    }

    public void Play(string animationName, bool loop = false)
    {
        if (_animation == null) return;
        _animation.AnimationState.SetAnimation(0, animationName, loop);
    }

  
}
