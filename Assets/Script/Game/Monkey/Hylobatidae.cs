using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hylobatidae : Monkey
{
    Rigidbody2D _rg = null;
    float _skillTime;
    bool _isBump = false;


    private void Awake()
    {
        _rg = GetComponent<Rigidbody2D>();
        Idle();

    }
    protected override void ShootEnd()
    {

        if (_isBump) return;

        if (isUseSkill)
            changeState(State.SkillBump);
        else
            changeState(State.Bump);

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

    public override void Shoot(float power, Vector2 direction)
    {
        if (_rg == null) return;
        _isBump = false;
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
        base.Skill();
        isUseSkill = true;

        _skillTime = Time.time;
        GameObject oj = loadPoolingObject(Def.PATH_OBJECT_ROCK, Def.OBJECT_ROCK);
        oj.transform.position = transform.position;
        oj.GetComponent<SkillRock>().Play(transform);
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
