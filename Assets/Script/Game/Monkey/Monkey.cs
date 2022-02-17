using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{

    private float _mass;
    private float _gravityScale;
    private float _angularDrag;

    public enum State { Idle, Ready, Shoot, ShootEnd, Bump, ComBack, Skill };
    public State state = State.Idle;
    [SerializeField] SpineRemote _spine = null;
    public bool isLeft = false;

    protected bool isUseSkill = false;

    const string ANIM_IDLE_1 = "idle_01";
    const string ANIM_IDLE_2 = "idle_02";
    const string ANIM_READY = "wing_00";
    const string ANIM_SHOOT = "wing_01";
    const string ANIM_BUMP = "wing_02";
    const string ANIM_COMBACK = "escape";

    public const string ANIM_SKILL = "skill";

    public bool IsGoHome { get { return _isGoHome; } }
    private bool _isGoHome = false;

    bool _isInit = false;
    private void Start()
    {

        Init();
    }


    public void OnEnable()
    {
        Init();
        StopAllCoroutines();
        _isGoHome = false;

        GetComponent<Rigidbody2D>().mass = _mass;
        GetComponent<Rigidbody2D>().angularDrag = _angularDrag;
        GetComponent<Rigidbody2D>().gravityScale = _gravityScale;

    }
    public void Init()
    {
        if (_isInit) return;
        _mass = GetComponent<Rigidbody2D>().mass;
        _angularDrag = GetComponent<Rigidbody2D>().angularDrag;
        _gravityScale = GetComponent<Rigidbody2D>().gravityScale;
        _isInit = true;
    }

    protected void AddAnimationPlay(string animationName)
    {
        _spine.AddPlay(animationName);
    }


    protected void changeState(State state,bool isAddAnim = false)
    {

        this.state = state;
        if (_spine == null) return;

        switch (state)
        {
            case State.Idle:
                    if (Random.value < 0.5f) _spine.Play(ANIM_IDLE_1, true);
                    else _spine.Play(ANIM_IDLE_2, false);
                
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
                if (!isAddAnim)
                    _spine.Play(ANIM_BUMP, false);
                else
                    _spine.AddPlay(ANIM_BUMP);
                
                    
                break;

            case State.Skill:
                _spine.Play(ANIM_SKILL, false);
                break;
        }
    }

    protected virtual void ShootEnd()
    {
        if (state == State.Shoot || state == State.Skill)
        {
            Game.I.ShootReady();
            changeState(State.ShootEnd);
        }
    }

    public virtual void Shoot(float power, Vector2 direction)
    {
        Init();
        changeState(State.Shoot);

        _isGoHome = false;
        Rigidbody2D rg = GetComponent<Rigidbody2D>();
        rg.angularVelocity = 0.0f;
        rg.angularDrag = _angularDrag;
        rg.mass = _mass;
        rg.gravityScale = _gravityScale;
        Vector2 vel = direction * power;
        rg.isKinematic = false;
        rg.velocity = vel;
    }


    public virtual void Shoot(Vector2 velocity)
    {
        _isGoHome = false;
        changeState(State.Shoot);
        Rigidbody2D rg = GetComponent<Rigidbody2D>();
        rg.angularVelocity = 0.0f;
        rg.gravityScale = 1.0f;
        rg.isKinematic = false;
        rg.velocity = velocity;

    }


    public virtual void Idle()
    {
        Init();
        changeState(State.Idle);
    }

    public virtual void Comback()
    {
        changeState(State.ComBack);
    }

    public virtual void Ready()
    {
        Init();
        Rigidbody2D rg = GetComponent<Rigidbody2D>();
        CircleCollider2D c = GetComponent<CircleCollider2D>();
        c.enabled = true;
        c.isTrigger = false;
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


        float time = 0.0f;

        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;

            if (time > 1.0f)
            {
                Rigidbody2D rg = GetComponent<Rigidbody2D>();
                if (rg.velocity.sqrMagnitude < 1f)
                {
                    CircleCollider2D c = GetComponent<CircleCollider2D>();
                    rg.velocity = Vector3.zero;
                    rg.angularVelocity = 0.0f;
                    rg.isKinematic = false;

                    GameObject oj = loadPoolingObject(Def.PATH_EFFECT_DUST1, Def.EFFECT_DUST1);
                    oj.transform.position = transform.position;

                    bool isRight = transform.localScale.y > 0 ? true : false;

                    if (isRight)
                        transform.localScale = Vector3.one;
                    else
                        transform.localScale = new Vector2(-1f, 1.0f);

                    transform.rotation = Quaternion.identity;
                    StartCoroutine(goHome(isRight));

                    yield break;
                }
            }

      


        }
    }
    IEnumerator goHome(bool isRight)
    {
        float time = 0.0f;
        GetComponent<CircleCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 7.0f;
        if(isRight)
             GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 500));
        else
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-10, 500));
        changeState(State.ComBack);
        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;
            Vector3 scale = transform.localScale;
            if (isRight)
            {
                scale.x += Time.deltaTime * 5.0f;
                scale.y += Time.deltaTime * 5.0f;
            }
            else
            {
                scale.x -= Time.deltaTime * 5.0f;
                scale.y += Time.deltaTime * 5.0f;
            }

            transform.localScale = scale;

            if (time > 3.0f)
            {
                transform.localScale = Vector3.one;
           
                GetComponent<CircleCollider2D>().isTrigger = false;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().isKinematic = true;
                GetComponent<Rigidbody2D>().gravityScale = _gravityScale;

                Destroy(gameObject);
                //GB.ObjectPooling.I.Destroy(gameObject);
                
                yield break;
            }
        }
    }

    float _trajectoryTime = 0.0f;

    public virtual void UpdateShoot()
    {

        if (state == State.Shoot || state == State.Skill)
        {
            _trajectoryTime += Time.deltaTime;

            if (_trajectoryTime > 0.1f)
            {
                GameObject oj = loadPoolingObject(Def.PATH_DOT_YELLOW, Def.DOTTED_YELLOW);
                oj.transform.position = transform.position;
                oj.transform.SetParent(null);
                oj.transform.localScale = new Vector2(0.5f, 0.5f);
                Game.I.SetTrailDotted(oj);
                _trajectoryTime = 0.0f;

            }

            Rigidbody2D rg = GetComponent<Rigidbody2D>();
            transform.right = rg.velocity;


            if (transform.position.y < -10.0f)
                ShootEnd();
            
        }
    }


    public virtual void OnCollisionEnter2D(Collision2D coll)
    {
        GameObject oj = loadPoolingObject(Def.PATH_EFFECT_DUST2, Def.EFFECT_DUST2);
        oj.transform.position = coll.contacts[0].point;

        if (state == State.Shoot)
        {
            oj = loadPoolingObject(Def.PATH_EFFECT_HIT, Def.EFFECT_HIT);
            oj.transform.position = coll.contacts[0].point;
        }


        ShootEnd();

        if (!_isGoHome)
        {
            StartCoroutine(endCheck());
            _isGoHome = true;
        }
    }

    protected GameObject loadPoolingObject(string path, string key)
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
