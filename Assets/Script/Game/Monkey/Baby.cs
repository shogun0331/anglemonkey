using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : Monkey
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
        _rg.isKinematic = true;
        base.Ready();
    }

    public override void Shoot(float power, Vector2 direction)
    {
        if (_rg == null) return;
        
        base.Shoot(power,direction);
        _rg.isKinematic = false;
        _rg.AddForce(direction * power);
    }

    public void UpdateComback(float dt)
    {

    }

    public override void Comback()
    {
        _rg.isKinematic = false;
        base.Comback();
    }

    void OnCollisionEnter(Collision coll)
    {
        ShootEnd();
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

}
