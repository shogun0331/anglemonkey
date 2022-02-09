using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{

    public enum State { Idle, Ready, Shoot, ShootEnd, Bump ,ComBack,Skill };
    public State state = State.Idle;
    [SerializeField] SpineRemote _spine = null;
    public bool isLeft = false;

    protected bool isUseSkill  = false;

    const string ANIM_IDLE_1 = "idle_01";
    const string ANIM_IDLE_2 = "idle_02";
    const string ANIM_READY = "wing_00";
    const string ANIM_SHOOT = "wing_01";
    const string ANIM_BUMP = "wing_02";
    const string ANIM_COMBACK = "escape";

    public const string ANIM_SKILL = "skill";

    public bool IsGoHome { get { return _isGoHome; } }
    private bool _isGoHome = false;

    private void changeState(State state)
    {
        this.state = state;
        if (_spine == null) return;

        switch (state)
        {
            case State.Idle:
                if (Random.value < 0.5f) _spine.Play(ANIM_IDLE_1, true);
                else                                 _spine.Play(ANIM_IDLE_2, false);
                break;

            case State.Ready:
                _spine.Play(ANIM_READY, true);
                break;

            case State.Shoot:
                _spine.Play(ANIM_SHOOT, true);
                break;

            case State.ComBack:
                _spine.Play(ANIM_COMBACK, false);
                break;

            case State.ShootEnd:
                _spine.Play(ANIM_BUMP, false);
                break;

            case State.Skill:
                _spine.Play(ANIM_SKILL, false);
                break;
        }
    }

    protected void ShootEnd()
    {
        changeState(State.ShootEnd);
    }
   
    public virtual void Shoot(float power, Vector2 direction)
    {
        changeState(State.Shoot);
        Rigidbody2D rg = GetComponent<Rigidbody2D>();
        Vector2 vel  = direction * power;
        rg.isKinematic = false;
        rg.velocity = vel;
    }

    public virtual void Idle()
    {
        changeState(State.Idle);
    }

    public virtual void Comback()
    {
        changeState(State.ComBack);
    }

    public virtual void Ready()
    {
        Rigidbody2D rg = GetComponent<Rigidbody2D>();
        CircleCollider2D c = GetComponent<CircleCollider2D>();
        c.enabled = true;
        rg.isKinematic = true;
        changeState(State.Ready);
    }

    public virtual void Skill()
    {
        changeState(State.Skill);
    }

    public virtual void UpdateWind(float power, Vector2 direction)
    {

    }

    IEnumerator endCheck()
    {
        if (IsGoHome) yield return null;
            
        bool checkEnd = false;
        float time = 0.0f;

        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;

            if (time > 2.0f)
            {
                Rigidbody2D rg = GetComponent<Rigidbody2D>();

                if (rg.velocity.sqrMagnitude < 0.2f)
                {
                    CircleCollider2D c = GetComponent<CircleCollider2D>();
                    rg.velocity = Vector3.zero;
                    rg.isKinematic = true;
                    rg.angularVelocity = 0.0f;

                    c.enabled = false;
                    checkEnd = true;
                }
            }

            if (checkEnd)
            {
                if (GB.ObjectPooling.I.GetRemainingUses(Def.EFFECT_DUST1) > 0)
                {
                    GameObject oj = GB.ObjectPooling.I.Import(Def.EFFECT_DUST1);
                    oj.transform.position = transform.position;
                }
                else
                {
                    GameObject resources = Resources.Load(Def.PATH_EFFECT_DUST1) as GameObject;
                    GameObject oj = Instantiate(resources);
                    GB.ObjectPooling.I.Registration(Def.EFFECT_DUST1, oj, true);
                    oj.transform.position = transform.position;
                }
                gameObject.SetActive(false);

                yield break;

                    
            }


        }
    }

    //할일 다했나 체크 
    public void UpdateEndCheck()
    {

        if (state == State.ShootEnd)
        {

            if (_isGoHome) return;
            

     
            
        }
    }

    IEnumerator goHomePlay()
    {
        float time = 0.0f;
        transform.rotation = Quaternion.identity;

        changeState(State.ComBack);

        Vector3 p1, p2, p3;
        p1 = transform.position;
        p2 = Camera.main.transform.position;
        p3 = Camera.main.transform.position;
        p3.y = -15.0f;

        while (true)
        {
           yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;

            if (time < 1.0f)
            {
                

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, time);

                if (time > 1.0f)
                {
                    gameObject.SetActive(false);
                    yield break;
                }
            }

        }
    }

    float _goHomeTime = 0.0f;
    public void UpdateGoHome()
    {









    }

    public virtual void UpdateShoot()
    {

        if (state == State.Shoot)
        {
            
            Rigidbody2D rg = GetComponent<Rigidbody2D>();
            transform.right = rg.velocity;
        }
    }


    public virtual void OnCollisionEnter2D(Collision2D coll)
    {

        if (GB.ObjectPooling.I.GetRemainingUses(Def.EFFECT_DUST2) > 0)
        {
            GameObject oj = GB.ObjectPooling.I.Import(Def.EFFECT_DUST2);
            oj.transform.position = coll.contacts[0].point;
        }
        else
        {
            GameObject resources = Resources.Load(Def.PATH_EFFECT_DUST2) as GameObject;
            GameObject oj = Instantiate(resources);
            GB.ObjectPooling.I.Registration(Def.EFFECT_DUST2, oj, true);
            oj.transform.position = coll.contacts[0].point;
        }



        ShootEnd();

        if (!_isGoHome)
        {
            StartCoroutine(endCheck());
            _isGoHome = true;
        }
    }


}
