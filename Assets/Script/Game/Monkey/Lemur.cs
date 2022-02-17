using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lemur : Monkey
{
    Rigidbody2D _rg = null;


    private void Awake()
    {
        _rg = GetComponent<Rigidbody2D>();
        Idle();

    }
    protected override void ShootEnd()
    {
        changeState(State.Bump, true);
        //AddAnimationPlay("wing_02");
        //base.ShootEnd();
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


    public override void Skill()
    {
        if (state != State.Shoot) return;
        if (isUseSkill) return;
        base.Skill();

        isUseSkill = true;

        float circleSize = 2f;
        float power = 2.2f;

        //범위 내에 있는 충돌체 모두 호출
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, circleSize);

        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].GetComponent<Brick>() != null)
            {
                loadPoolingObject(Def.PATH_EFFECT_DUST2, Def.EFFECT_DUST2).transform.position = colls[i].transform.position;
                colls[i].GetComponent<Brick>().SetDamage(power);
            }
        }
      
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
