using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Macaca : Monkey
{
    Rigidbody2D _rg = null;

    [SerializeField] SpineRemote _skill = null;
    [SerializeField] TrailRenderer _trail = null;

    private void Awake()
    {
        _rg = GetComponent<Rigidbody2D>();
        Idle();
        _skill.gameObject.SetActive(false);
    }

    public override void Idle()
    {
        _rg.isKinematic = true;
        base.Idle();
    }

    public override void Ready()
    {
        isUseSkill = false;
        _skill.gameObject.SetActive(false);
        _trail.enabled = false;
        base.Ready();
    }
    protected override void ShootEnd()
    {
        base.ShootEnd();
    }
    public override void Shoot(float power, Vector2 direction)
    {
        if (_rg == null) return;
        _skill.gameObject.SetActive(false);
        _trail.enabled = false;
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
        loadPoolingObject(Def.PATH_EFFECT_DUST2, Def.EFFECT_DUST2).transform.position = transform.position;
        //중력값 감소
        _rg.gravityScale = 0.5f;
        _rg.velocity = _rg.velocity.normalized * _rg.velocity.magnitude * 2.5f;
        
        _skill.gameObject.SetActive(true);
        _skill.Play("skill");
        _trail.enabled = true;
        
    }

    public override void UpdateWind(float power, Vector2 direction)
    {
        if (state == State.Shoot || state == State.Skill)
            _rg.AddForce(power * direction);
    }

    public override void OnCollisionEnter2D(Collision2D coll)
    {
        base.OnCollisionEnter2D(coll);
        _skill.gameObject.SetActive(false);
    }


    private GameObject loadPoolingObject(string path, string key)
    {
        GameObject oj = null;
        if (GB.ObjectPooling.I.GetRemainingUses(key) > 0)
        {
            oj = GB.ObjectPooling.I.Import(key);
        }
        else
        {
            GameObject resources = null;

            if (GB.ObjectPooling.I.CheckModel(key))
            {
                resources = GB.ObjectPooling.I.GetModel(key);
                oj = Instantiate(resources);
            }
            else
            {
                resources = Resources.Load<GameObject>(path);
                GB.ObjectPooling.I.RegistModel(key, resources);
                oj = Instantiate(resources);
            }

            GB.ObjectPooling.I.Registration(key, oj, true);
        }

        return oj;
    }

}
