using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hylobatidae : Monkey
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



    /// <summary>
    /// Baby는 스킬 없음
    /// </summary>
    public override void Skill()
    {
        if (state != State.Shoot) return;
        if (isUseSkill) return;
        isUseSkill = true;
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
