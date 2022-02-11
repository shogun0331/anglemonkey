using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineRemote : MonoBehaviour
{
    private SkeletonAnimation _animation;

    [SerializeField] bool _isLoop = false;
    [SerializeField] float _loopDelay = 0.0f;

    float _time = 0.0f;

    private void Awake()
    {
        _animation = GetComponent<SkeletonAnimation>();
    }

    private void OnEnable()
    {
        _time = 0.0f;
    }

    public void Play(string animationName, bool loop = false)
    {
        if (_animation == null) return;
        _animation.AnimationState.SetAnimation(0, animationName, loop);
    }

    private void Update()
    {
        if (!_isLoop) return;

        _time += Time.deltaTime;

        if (_time > _loopDelay)
            _time = 0.0f;
    }

}
