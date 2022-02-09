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

    



    private void playDestroy(float force)
    {
        StartCoroutine(playDestroyAction(force));
    }


    IEnumerator playDestroyAction(float force)
    {

        const int ACTION_MOVE_CENTER = 0;
        const int ACTION_MOVE_DOWN = 1;


        float dt = 0.02f;

        if (_sprRenderModel != null)
            _sprRenderModel.sortingOrder = 1;

        Vector2 center = Camera.main.transform.position;
        Vector2 targetScale = _orijinScale * 3f;
        
        float time = 0.0f;

        int action = ACTION_MOVE_CENTER;

        float speed = force * 0.01f;


        while (true)
        {
            yield return new WaitForSeconds(dt);



            time += dt + (speed * dt);

            if (time > 0.8f)
            {
            }

            if (time > 1.0f)
            {

                yield break;
            }
        }
    }

    public void SetModel(ModelBrick brick)
    {

        _model = brick;
        _hp = _model.Hp;
        _score = _model.Score;
        _isDestroy = false;


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

    private void DestroyBrick()
    {

        _isDestroy = true;
        //GameObject oj = null;
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

        gameObject.SetActive(false);

        //애니메이션 할지 결정
        if (Random.value > 0.8f)
        {
            //gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            //playDestroy(magnitude);
        }
        else
        {

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


    void OnCollisionEnter2D(Collision2D coll)
    {
        float magnitude = coll.relativeVelocity.sqrMagnitude;

        Rigidbody2D rg = GetComponent<Rigidbody2D>();
        if (rg == null) return;
        if (coll.rigidbody == null ) return;
       
        float damage = 0.0f;

        //대상의 충돌 속도와 본인의 충돌 속도 중 더 크기가 큰쪽으로 변수를 할당한다.
        damage = Mathf.Max(coll.relativeVelocity.magnitude * coll.rigidbody.mass, rg.velocity.magnitude * rg.mass);


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
        {
            DestroyBrick();
        }
        else
            _hp -= damage;
    }


}
