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

        float speed = 5.5f ;
        float angle = 30.0f;
        float distance = 2;
        float power = 2;
        //���� ������ �ӵ��� ũ�Ⱚ
        float mag = _rg.velocity.magnitude;

        //���� ���� �ִ� �浹ü ��� ȣ��
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, distance);

        //Qu
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);  // ȸ����
        if (_rg.velocity.x < 0)
            rot = Quaternion.Euler(0f, 0f, -180 - angle);
        Vector3 rotatedDirection = rot * new Vector3(1, 0, 0);

        //�ϴ� �ֺ��� �浹ü���ֵ� ���� ������ �̵���θ��ٲ����
        _rg.velocity = rotatedDirection * speed * 1.5f;
        //�������� �ƹ��͵� ���ٸ� ����
        if (colls.Length == 1)
            return;
        
        for (int i = 0; i < colls.Length; i++)
        {
            //�浹ü�� �ڱ� �ڽ��̰ų� �浹ü�� Rigdbody2D�� ���ٸ� �������� 
            if (colls[i].gameObject == this.gameObject || colls[i].TryGetComponent(out Rigidbody2D _rigid) == false)
                continue;

            //������135~225�����϶�
            if (Quaternion.FromToRotation(Vector3.right, colls[i].transform.position - transform.position).eulerAngles.z > 270 - 45 && Quaternion.FromToRotation(Vector3.right, colls[i].transform.position - transform.position).eulerAngles.z < 270 + 45)
            {
                //������ ���� ����
                Vector2 vec2;
                vec2.x = (colls[i].transform.position - transform.position).normalized.x;
                vec2.y = (colls[i].transform.position - transform.position).normalized.y;

                //���� * �� * �Ÿ� * (1/����)
                _rigid.velocity += (vec2 * power) * (1 / _rigid.mass);

                if (colls[i].TryGetComponent(out Brick _brick))
                    _brick.SetDamage(power);
            }
        }

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
