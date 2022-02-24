using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Monkey
{

    Rigidbody2D _rg = null;


    private void Awake()
    {
        _rg = GetComponent<Rigidbody2D>();
        Idle();
    }

    public override void Idle()
    {
        _rg.isKinematic = true;
        base.Idle();
    }

    public override void Ready()
    {
        isUseSkill = false;

        base.Ready();
    }
    protected override void ShootEnd()
    {
        base.ShootEnd();
    }
    public override void Shoot(float power, Vector2 direction)
    {
        if (_rg == null) return;
        base.Shoot(power, direction);
    }

    public override void UpdateShoot()
    {
        base.UpdateShoot();

    }

    public override void Comback()
    {
       _rg.isKinematic = false;
        base.Comback();

    }


    public override void Skill()
    {
        if (state != State.Shoot) return;
        if (isUseSkill) return;

        isUseSkill = true;

        //분신 생성
        GameObject m1 = loadPoolingObject(Def.PATH_MONKEY_GOLD, Def.MONKEY_GOLD);
        GameObject m2 = loadPoolingObject(Def.PATH_MONKEY_GOLD, Def.MONKEY_GOLD);

        m1.transform.position = transform.position;
        m2.transform.position = transform.position;

        //=========================원본 가져온내용

        //20
        float power = _rg.velocity.magnitude;
        Vector3 dir = _rg.velocity.normalized;

        float addAngle = 20.0f;

        //+Angle분신
        Quaternion upRotation = Quaternion.Euler(0f, 0f, addAngle);  
        Vector3 upRotatedDirection = upRotation * dir;//
        upRotatedDirection = upRotatedDirection * power;

        //-Angle분신
        Quaternion downRotation = Quaternion.Euler(0f, 0f, -addAngle);
        Vector3 downRotatedDirection = downRotation * dir;
        downRotatedDirection = downRotatedDirection * power;

        //===================================

        m1.GetComponent<Monkey>().Shoot(upRotatedDirection);
        m2.GetComponent<Monkey>().Shoot(downRotatedDirection);



        loadPoolingObject(Def.PATH_EFFECT_DUST2, Def.EFFECT_DUST2).transform.position = transform.position;
        
            


    }

    public override void UpdateWind(float power, Vector2 direction)
    {
        if (state == State.Shoot || state == State.Skill)
            _rg.AddForce(power * direction);
    }

    public override void OnCollisionEnter2D(Collision2D coll)
    {
        base.OnCollisionEnter2D(coll);
    }

}
