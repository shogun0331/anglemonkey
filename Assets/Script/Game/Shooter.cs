using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public bool isTest = false;

    public enum State { NotReady = 0,Ready, Aim }
    public State state = State.NotReady;
    
    [Header("Band")]
    [SerializeField] LineRenderer _leftBandLine = null;
    [SerializeField] LineRenderer _rightBandLine = null;
    [SerializeField] GameObject _imgIdleBand = null;
    [SerializeField] GameObject _imgAimBand = null;
    [SerializeField] Transform _aimPoint = null;

    float _maxBandLength = 2;
    Vector3 _dragPosition;
    [SerializeField]    GameObject _bullet;
    public GameObject Bullet { get { return _bullet; } }
    

[Header("Particle")]
    [SerializeField] ParticleSystem _shootParticle = null;

    //[Header("GuideLine")]
    //[SerializeField]  GuideLine _guideLine = null;
    [Header("Dotted GuideLine")]
    [SerializeField] DottedGuide _dottedGuide = null;

    int _maxGuidLineCount = 20;

    [Range(1.0f,20.0f)]
    [SerializeField] float _shootPower = 20.0f;
    
  

    private void Awake()
    {

        Init();

    }




    [SerializeField] GameObject TestOBJ = null;

    private void drawGuideLine(Vector2 direction,float force)
    {
        //if (_bullet == null) return;

        //G Grivity
        //T Time;
        //V Velocity
        //S StartPosition

        Vector2 S = _aimPoint.position;
        Vector2 V = direction * force;
        float dt = 0.07f;
        float T = 0.0f;

        float G =  Mathf.Abs( Physics2D.gravity.y);
        List<Vector2> list = new List<Vector2>();
        for (int i = 0; i < 10; ++i)
        {
            float x = S.x + V.x * T;
            float y = S.y + V.y * T - 0.5f * G * T * T;
            T += dt;
            
            Vector2 pos = new Vector2(x, y);
            list.Add(pos);
            
        }



        float dist = Vector2.Distance(list[0], list[1]);
        ////Draw
        if (!_dottedGuide.IsReady)
            _dottedGuide.Ready(list, DottedGuide.Color.Blue,true);
        else
            _dottedGuide.DrawAnimation(list, Time.deltaTime );


    }

    private void clearGuideLine()
    {
        _dottedGuide.Clear();
    }

    /// <summary>
    /// 초기 상태
    /// </summary>
    public void Init()
    {
        _leftBandLine.positionCount = 2;
        _rightBandLine.positionCount = 2;

        _leftBandLine.SetPosition(0, _leftBandLine.transform.position);
        _rightBandLine.SetPosition(0, _rightBandLine.transform.position);

        _leftBandLine.SetPosition(1, _leftBandLine.transform.position);
        _rightBandLine.SetPosition(1, _rightBandLine.transform.position);

        _bullet = null;
        state = State.NotReady;

        clearGuideLine();

        _imgAimBand.SetActive(false);
        _imgIdleBand.SetActive(true);
    }

    /// <summary>
    /// 장전
    /// </summary>
    /// <param name="bullet">총알 장전</param>
    public void SetReady(GameObject bullet)
    {
        _bullet = bullet;
        _bullet.transform.SetParent(null);
        _bullet.transform.position = Vector3.zero;
        _bullet.transform.localScale = Vector3.one;
        _bullet.transform.rotation = Quaternion.identity;

        _bullet.transform.SetParent(_aimPoint);
        _bullet.transform.localPosition = Vector3.zero;
        _bullet.transform.localPosition = new Vector2(0.2f, -0.08f);
        _bullet.SetActive(true);

        state = State.Ready;
    }


    /// <summary>
    /// Aming
    /// </summary>
    /// <param name="point">터치 포인트</param>
    public void SetAming(Vector3 point)
    {
        if (!isTest && _bullet == null ) return;
        if (state != State.Aim) return;

        if (_imgIdleBand.activeSelf == true)
            _imgIdleBand.SetActive(false);

        if (_imgAimBand.activeSelf == false)
            _imgAimBand.SetActive(true);

        if (_bullet != null)
        {
            _bullet.transform.localRotation = Quaternion.identity;
            _bullet.transform.SetParent(_imgAimBand.transform);
            _bullet.transform.localPosition = new Vector3(0.3f, 0.1f, 0.0f);

        }

        _dragPosition = _aimPoint.position + Vector3.ClampMagnitude(point - _aimPoint.position, _maxBandLength);

        //Shooter 바디에 가까워질수록 각도는 줄어듬
        //float checkAngleX =   7.5f;
        
        //if (_dragPosition.y < _aimPoint.position.y
        //    && _dragPosition.x < _aimPoint.position.x + checkAngleX
        //    && _dragPosition.x > _aimPoint.position.x - checkAngleX)
        //{
        
        //    float minLength = 0.43f;

            

        //    //0~1
        //    float distPer = Mathf.Abs(_dragPosition.x) / checkAngleX;

        //    float length = minLength * distPer;

            
        //    Debug.Log(distPer);

        //    //float minX = 0.2f;
        //    //if (length < minX) length = minLength;

        //    _dragPosition = _aimPoint.position + Vector3.ClampMagnitude(point - _aimPoint.position, length);
        //}
 

        //_dragPosition = point;
        //_dragPosition = _aimPoint.position + Vector3.ClampMagnitude(point - _aimPoint.position, _maxBandLength);
        
        //연출
        //=====================================================================


        //밴드 끝 부분 에임 포인트 회전
        Vector2 dir = (_aimPoint.position - point);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion axis = Quaternion.AngleAxis(angle, Vector3.forward);
        _imgAimBand.transform.rotation = axis;

        if (_bullet != null)
        {
            if (_dragPosition.x > _aimPoint.position.x)
                _bullet.transform.localScale = new Vector3(1.0f, -1.0f, 1.0f);
            else
                _bullet.transform.localScale = Vector3.one;
            
        }

        //=====================================================================
        _imgAimBand.transform.position = _dragPosition;

        _leftBandLine.SetPosition(0,_leftBandLine.transform.position);
        _leftBandLine.SetPosition(1, _dragPosition);
        _rightBandLine.SetPosition(0, _rightBandLine.transform.position);
        _rightBandLine.SetPosition(1, _dragPosition);

        drawGuideLine(dir.normalized, GetPower() * _shootPower);
        
    }

    /// <summary>
    /// 슛
    /// </summary>
    public void Shoot()
    {
        if (state != State.Aim) return;

        if (_shootParticle != null)
        {
            _shootParticle.transform.position = _aimPoint.position;
            _shootParticle.Play();
        }

        if (_bullet != null)
        {
            _bullet.transform.position = _aimPoint.transform.position;
            _bullet.transform.SetParent(null);
            _bullet.GetComponent<Monkey>().Shoot(GetPower() * _shootPower , GetDirection());
        }
        
        Init();
    }

    /// <summary>
    /// 슛 방향
    /// </summary>
    /// <returns>방향</returns>
    public Vector3 GetDirection()
    {
        Vector3 dir = (_aimPoint.position - _dragPosition).normalized;
        return dir;
    }

    /// <summary>
    /// 슛 파워 
    /// </summary>
    /// <returns>퍼센트</returns>
    public float GetPower()
    {
        float dist = Vector2.Distance(_dragPosition, _aimPoint.position);
        float percent = dist / _maxBandLength;

        return percent ;
    }

  



    /// <summary>
    /// 테스트
    /// </summary>
    private void Update()
    {

        if (Input.GetKeyUp(KeyCode.Z))
        {
            
            isTest = true;
            if (_bullet == null)
            {

                if (GB.ObjectPooling.I.GetRemainingUses("BABY") > 0)
                {
                    GameObject baby = GB.ObjectPooling.I.Import("BABY");
                    SetReady(baby);

                }
                else
                {
                    GameObject baby = Resources.Load<GameObject>(Def.PATH_MONKEY_BABY);
                    GameObject oj = Instantiate(baby);
                    SetReady(oj);
                    GB.ObjectPooling.I.Registration("BABY", oj, true);
                }
            }

        }


        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            if (state == State.Ready)
            {
                float dist = Vector2.Distance(mousePosition, transform.position);
                if (dist > 0.8f) return;
                else state = State.Aim;
            }
            
            SetAming(mousePosition);


        }

        if (Input.GetMouseButtonUp(0))
        {

            if (isTest)
            {
                //GB.ObjectPooling.I.Destroy(_bullet);
            }

            Shoot();
        }

        


    }


}
