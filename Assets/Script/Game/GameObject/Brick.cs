using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private ModelBrick _model;
    public enum Shape {Rectangle1 = 0, Rectangle2, Rectangle3, Rectangle4, Triangle,Righttriangle, Square, Circle , QCircle ,End};
    [SerializeField] Shape _shape;
    public Shape MyShape{ get{ return _shape;}}
    public enum OutsideTexture { Ice = 0, Iron,Stone,Wood,End};
    [SerializeField] OutsideTexture _texture;
    public OutsideTexture MyTexture{get{ return _texture;}}

    [SerializeField] Sprite _sprOrijin = null;
    [SerializeField] Sprite _sprBroken = null;

    [SerializeField] float _hp;
    [SerializeField] int _score;

    bool _isDestroy = false;
    public bool IsDestroy { get { return _isDestroy; } }

    SpriteRenderer _sprRenderModel = null;

    Vector2 _orijinScale;


    private CircleCollider2D _cCollider = null;
    private PolygonCollider2D _pCollider = null;
    private BoxCollider2D _bCollider = null;


    public void SetModel(ModelBrick brick)
    {
        
        _model = brick;
        _hp = _model.Hp;
        _score = _model.Score;
        _isDestroy = false;

        setTriger(false);
        _orijinScale = transform.localScale;

        if (GetComponent<SpriteRenderer>() == null)
        {
            Transform child = transform.GetChild(0);
            if (child != null)
                _sprRenderModel = child.GetComponent<SpriteRenderer>();
        }
        else
        {
            _sprRenderModel = GetComponent<SpriteRenderer>();
        }

        if (_sprRenderModel != null)
        {
            _sprRenderModel.sprite = _sprOrijin;
            _sprRenderModel.sortingOrder = 0;
        }

        StopAllCoroutines();

    }

    private void setTriger(bool isActive)
    {

        _cCollider = GetComponent<CircleCollider2D>();
        if (_cCollider != null) _cCollider.isTrigger = isActive;

        _bCollider = GetComponent<BoxCollider2D>();
        if (_bCollider != null) _bCollider.isTrigger = isActive;

        _pCollider = GetComponent<PolygonCollider2D>();
        if (_pCollider != null) _pCollider.isTrigger = isActive;
    }

    /// <summary>
    /// 내부 맵 JSON 파일을 강제적으로 수정한 경우 프리팹 데이터랑 비교
    /// </summary>
    /// <returns>데이터 변조</returns>
    public bool CheckMemoryHack()
    {
        if (_model.Hp != _hp)                                                                                   return true;
        if (_model.Score != _score)                                                                         return true;
        if (_model.Mass != GetComponent<Rigidbody2D>().mass)                        return true;
        if (_model.GravityScale != GetComponent<Rigidbody2D>().gravityScale) return true;
        if (_model.AngularDrag != GetComponent<Rigidbody2D>().angularDrag) return true;

        return  false;
    }

    private void DestroyBrick(float damage)
    {

        _isDestroy = true;

        Game.I.AddScore(transform.position,_model.Score);
        
        switch (MyTexture)
        {
            case OutsideTexture.Ice:
                loadPoolingObject(Def.PATH_EFFECT_ICE, Def.EFFECT_ICE).transform.position = transform.position;
                break;
            case OutsideTexture.Stone:
                loadPoolingObject(Def.PATH_EFFECT_ROCK, Def.EFFECT_ROCK).transform.position = transform.position;
                break;
            case OutsideTexture.Wood:
                loadPoolingObject(Def.PATH_EFFECT_WOOD, Def.EFFECT_WOOD).transform.position = transform.position;
                break;
        }

        //애니메이션 할지 결정 
        if (damage > 10.0f &&  Random.value > 0.8f)
        {
            setTriger(true);
            StartCoroutine(playDestroyAction(damage));
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    IEnumerator playDestroyAction(float damage)
    {
        float time = 0.0f;

        Vector2 orijin = transform.localScale;

        float x = Random.Range(-500.0f, 500.0f);
        float y = Random.Range(1000.0f, 2000.0f);

        GetComponent<Rigidbody2D>().angularVelocity = Random.value > 0.5 ? damage * 20.0f : -damage * 20.0f;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(x, y));
        GetComponent<Rigidbody2D>().gravityScale = 5;



        if (_sprRenderModel != null)
            _sprRenderModel.sortingOrder = 1000;
     
        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;

            Vector2 scale = transform.localScale;
            scale.x += Time.deltaTime * 3;
            scale.y += Time.deltaTime * 3;
            transform.localScale = scale;

            if (time > 3.0f)
            {
                if (_sprRenderModel != null)
                    _sprRenderModel.sortingOrder = 0;
                transform.localScale = orijin;
                transform.rotation = Quaternion.identity;
                GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().gravityScale = _model.GravityScale;
                setTriger(false);
                gameObject.SetActive(false);
                yield break;
            }


        }

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
            GameObject resources = Resources.Load<GameObject>(path);
            oj = Instantiate(resources);
            GB.ObjectPooling.I.Registration(key, oj, true);
        }

        return oj;
    }



    public void SetDamage(float damage)
    {
        float hp = _hp - damage;
      

        if (_model != null)
        {
            if (hp < _model.Hp * 0.5f)
            {
                if (_sprRenderModel != null && _sprBroken != null)
                    _sprRenderModel.sprite = _sprBroken;
            }
        }

        if (hp < 0.0f)
            DestroyBrick(damage);
        else
            _hp -= damage;


    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!Game.I.GetReady()) return;
        if (_isDestroy) return;

        float magnitude = coll.relativeVelocity.sqrMagnitude;

        Rigidbody2D rg = GetComponent<Rigidbody2D>();
        if (rg == null) return;
        if (coll.rigidbody == null ) return;
       
        float damage = 0.0f;

        //대상의 충돌 속도와 본인의 충돌 속도 중 더 크기가 큰쪽으로 변수를 할당한다.
        damage = Mathf.Max(coll.relativeVelocity.magnitude * coll.rigidbody.mass, rg.velocity.magnitude * rg.mass);
        SetDamage(damage);


    }


}
