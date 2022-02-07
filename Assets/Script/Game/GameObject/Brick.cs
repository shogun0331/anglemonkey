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

    [SerializeField] float _hp;
    [SerializeField] int _score;

    bool _isDestroy = false;
    public bool IsDestroy { get { return _isDestroy; } }

    SpriteRenderer _sprRenderModel = null;

    Vector2 _orijinScale;

    

    private void Start()
    {
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


        if(_sprRenderModel != null)
        _sprRenderModel.sortingOrder = 0;

    }


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
        _isDestroy = false;

        Transform child = transform.GetChild(0);
        if (child != null)
            _sprRenderModel = child.GetComponent<SpriteRenderer>();

        if (_sprRenderModel != null)
            _sprRenderModel.sortingOrder = 0;

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


    void OnCollisionEnter2D(Collision2D coll)
    {

        float magnitude = coll.relativeVelocity.sqrMagnitude;
        float damage = magnitude * 0.1f;
        float hp = _hp - damage;


        if (hp < 0.0f)
        {
            //애니메이션 할지 결정
            //if (Random.value > 0.8f)
            //{
            //    playDestroy(magnitude);
            //}
            //else
            {
                gameObject.SetActive(false);
            }
            
            _isDestroy = true;
        }
        else
            _hp -= damage;
    }


}
