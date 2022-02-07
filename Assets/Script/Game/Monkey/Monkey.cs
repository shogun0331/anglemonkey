using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{

    public enum State { Idle, Ready, Shoot, ShootEnd, Bump ,ComBack,Skill };
    public State state = State.Idle;
    [SerializeField] SpineRemote _spine = null;

    protected bool isUseSkill  = false;

    const string ANIM_IDLE_1 = "idle_01";
    const string ANIM_IDLE_2 = "idle_02";
    const string ANIM_READY = "wing_00";
    const string ANIM_SHOOT = "wing_01";
    const string ANIM_BUMP = "wing_02";
    const string ANIM_COMBACK = "escape";

    public const string ANIM_SKILL = "skill";

    private void changeState(State state)
    {
        this.state = state;
        if (_spine == null) return;

        switch (state)
        {
            case State.Idle:
                if (Random.value < 0.5f) _spine.Play(ANIM_IDLE_1, true);
                else                                 _spine.Play(ANIM_IDLE_2, false);
                break;

            case State.Ready:
                _spine.Play(ANIM_READY, true);
                break;

            case State.Shoot:
                _spine.Play(ANIM_SHOOT, true);
                break;

            case State.ComBack:
                _spine.Play(ANIM_COMBACK, false);
                break;

            case State.ShootEnd:
                _spine.Play(ANIM_COMBACK, false);
                break;

            case State.Skill:
                _spine.Play(ANIM_SKILL, false);
                break;
        }
    }

    protected void ShootEnd()
    {
        changeState(State.ShootEnd);
    }
   
    public virtual void Shoot(float power, Vector2 direction)
    {
        changeState(State.Shoot);
        Rigidbody2D rg = GetComponent<Rigidbody2D>();
        
        Vector2 vel  = direction * power;
        rg.isKinematic = false;
        rg.velocity = vel;
    }

    public virtual void Idle()
    {
        changeState(State.Idle);
    }

    public virtual void Comback()
    {
        changeState(State.ComBack);
    }

    public virtual void Ready()
    {
        Rigidbody2D rg = GetComponent<Rigidbody2D>();
        rg.isKinematic = true;
        changeState(State.Ready);
    }

    public virtual void Skill()
    {
        changeState(State.Skill);
    }

    public virtual void UpdateWind(float power, Vector2 direction)
    {

    }


    public virtual void UpdateRotation()
    {
        if (state != State.Shoot) return;

        Rigidbody2D rg = GetComponent<Rigidbody2D>();
        transform.right = rg.velocity;
    }


    public virtual void OnCollisionEnter2D(Collision2D coll)
    {
        ShootEnd();
    }


}
