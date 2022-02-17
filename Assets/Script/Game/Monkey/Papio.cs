using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Papio : Monkey
{
    Rigidbody2D _rg = null;
    
    [SerializeField] SpineRemote _skill = null;

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
    protected override void ShootEnd()
    {
        base.ShootEnd();
    }
    public override void Ready()
    {
        isUseSkill = false;
        _skill.gameObject.SetActive(false);
        base.Ready();
    }

    public override void Shoot(float power, Vector2 direction)
    {
        if (_rg == null) return;
        _skill.gameObject.SetActive(false);
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
        _skill.gameObject.SetActive(true);
        _skill.Play("ef_01");

        float circleSize = 3.0f;
        float power = 3.5f;

        //���� ���� �ִ� �浹ü ��� ȣ��
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, circleSize);

        //�������� �ƹ��͵� ���ٸ� ����
        if (colls.Length == 0)
            return;
        Vector3 nowAngle = transform.rotation.eulerAngles;

        float min, max;
        min = nowAngle.z - 45;
        max = nowAngle.z + 45;
        bool isAnd = true;
        if (min < 0)
        {
            min += 360;
            isAnd = false;
        }

        if (max > 360)
        {
            max -= 360;
            isAnd = false;
        }
        // Debug.Log("Papio::usingSkil");
        for (int i = 0; i < colls.Length; i++)
        {
            //�浹ü�� �ڱ� �ڽ��̰ų� �浹ü�� Rigdbody2D�� ���ٸ� �������� 
            if (colls[i].gameObject == this.gameObject || colls[i].GetComponent<Rigidbody2D>() == null)
                continue;
            if (isAnd)
            {

                //Debug.Log("ROT : " + nowAngle.z + ", MIN : " + min + "&& MAX : " + max);

                //���ΰ� �ٸ� ��ü������ ������ ������ ������ �������̿� ���ϴ°�?
                if (Quaternion.FromToRotation(Vector3.right, colls[i].transform.position - transform.position).eulerAngles.z < max && Quaternion.FromToRotation(Vector3.right, colls[i].transform.position - transform.position).eulerAngles.z > min)
                {
                    //Debug.Log("isAnd DIST : " + Quaternion.FromToRotation(transform.position, colls[i].transform.position).eulerAngles.z+", NAME : "+ colls[i].gameObject.name);

                    //�ڽ��� ��ֺ��� 
                    Vector3 nor = _rg.velocity.normalized;

                    //������ �Ÿ�(����)
                    float mag = 1.0f;
                    //= Vector3.Distance(transform.position, colls[i].transform.position);


                    //������ ���� ����
                    Vector2 vec2;
                    vec2.x = (colls[i].transform.position - transform.position).normalized.x;
                    vec2.y = (colls[i].transform.position - transform.position).normalized.y;

                    //���� * �� * �Ÿ� * (1/����)
                    colls[i].GetComponent<Rigidbody2D>().velocity += (vec2 * power) * (1 / colls[i].GetComponent<Rigidbody2D>().mass) * (1 / mag);

                    if (colls[i].GetComponent<Brick>() != null)
                        colls[i].GetComponent<Brick>().SetDamage( power);
                }
            }
            else
            {
                //������ 0~45/ 315~360�����϶�
                if (Quaternion.FromToRotation(Vector3.right, colls[i].transform.position - transform.position).eulerAngles.z < max || Quaternion.FromToRotation(Vector3.right, colls[i].transform.position - transform.position).eulerAngles.z > min)
                {
                    //�ڽ��� ��ֺ��� 
                    Vector3 nor = _rg.velocity.normalized;

                    //������ �Ÿ�(����)
                    float mag = 1.0f;
                    //= Vector3.Distance(transform.position, colls[i].transform.position);

                    //������ ���� ����
                    Vector2 vec2;
                    vec2.x = (colls[i].transform.position - transform.position).normalized.x;
                    vec2.y = (colls[i].transform.position - transform.position).normalized.y;

                    //���� * �� * �Ÿ� * (1/����)
                    colls[i].GetComponent<Rigidbody2D>().velocity += (vec2 * power) * (1 / colls[i].GetComponent<Rigidbody2D>().mass) * (1 / mag);

                    if (colls[i].GetComponent<Brick>() != null)
                        colls[i].GetComponent<Brick>().SetDamage(power);
                }
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
