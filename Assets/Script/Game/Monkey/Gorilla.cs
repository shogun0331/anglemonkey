using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gorilla : Monkey
{
    Rigidbody2D _rg = null;

    [SerializeField] SpineRemote _skillSpine = null;

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
        if (!isUseSkill)
        {
            base.UpdateShoot();
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
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

        _skillSpine.gameObject.SetActive(true);
        _skillSpine.Play("skill",true);
        _rg.velocity = Vector2.down * 20f;

        transform.rotation = Quaternion.identity;



        

        ////Left
        if (isLeft)
            transform.localScale = new Vector2(-1f, 1.0f);
        else
            transform.localScale = Vector3.one;
            
        
        transform.rotation = Quaternion.identity;


    }

    public override void UpdateWind(float power, Vector2 direction)
    {
        if (state == State.Shoot || state == State.Skill)
            _rg.AddForce(power * direction);
    }
    bool _isCrash = false;

    public override void OnCollisionEnter2D(Collision2D coll)
    {
        base.OnCollisionEnter2D(coll);

        if (isUseSkill && !_isCrash)
        {

             loadPoolingObject(Def.PATH_EFFECT_DUST1, Def.EFFECT_DUST1).transform.position = coll.contacts[0].point;
            //충돌좌표
            Vector2 v2pos;
            v2pos = coll.contacts[0].point;
        
            Collider2D[] colls = Physics2D.OverlapCircleAll(v2pos, 2.0f);

            Vector3 v3pos = coll.contacts[0].point;

            for (int i = 0; i < colls.Length; i++)
            {
                //충돌체가 자기 자신이거나 rigidbody2d가 없으면 다음으로
                if (colls[i].gameObject == this.transform.gameObject || colls[i].TryGetComponent(out Rigidbody2D _rigid) == false)
                    continue;

                //노멀벡터 
                Vector3 colPosChange = colls[i].transform.position;

                Vector3 nor = (colPosChange - v3pos).normalized;

                //대상과의 거리
                float mag = 1f;
                //= Vector3.Distance(v3pos, colls[i].transform.position);

                //방향 * 힘 * 거리 * (질량/1)
                _rigid.velocity += new Vector2(nor.x, Mathf.Abs(nor.y)) * 5.0f * (1 / mag) * (1 / _rigid.mass);
            }

            _isCrash = true;

            _skillSpine.gameObject.SetActive(false);




        }

    }
}
