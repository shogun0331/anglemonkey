using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nasalis : Monkey
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
        base.Skill();

        isUseSkill = true;

        _rg.AddForce(Vector2.up * 1000.0f);

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
