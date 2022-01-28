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
    GameObject _bullet;

    [Header("Particle")]
    [SerializeField] ParticleSystem _shootParticle = null;

    [Header("GuideLine")]
    [SerializeField]  GuideLine _guideLine = null;
    int _maxGuidLineCount = 20;

    [Range(5.0f,50.0f)]
    [SerializeField] float _shootPower = 5.0f;
      
    private void Awake()
    {
        Init();
    }

    private void drawGuideLine(Vector2 direction,float force)
    {
        //if (_bullet == null) return;

        float mass = 1.0f;



        float dt = 0.02f;

        float weight = 1;
        Vector2 gravity = Physics2D.gravity;
        gravity.y -= weight;
        List<Vector3> list = new List<Vector3>();
        float time = 0;

        for (int i = 0; i < _maxGuidLineCount; ++i)
        {
            Vector2 pos = (Vector2)_aimPoint.position + (direction * force * time) + 0.5f * gravity  * (time * time);
            time += dt  * 3;

            if( i > 0 )
            list.Add(pos);

        }

        _guideLine.SetPositions(list);
        _guideLine.UpdateOffset(dt);

    }

    private void clearGuideLine()
    {
        //_guideLine.Clear();
    }

    /// <summary>
    /// 초기 상태
    /// </summary>
    private void Init()
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
    private void SetReady(GameObject bullet)
    {
        _bullet = bullet;
        _bullet.transform.parent = _aimPoint;
        _bullet.transform.rotation = Quaternion.identity;
        _bullet.transform.localPosition = new Vector2(0.5f, -0.5f);

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



        //Shooter 바디에 가까워질수록 각도는 줄어듬
        //float checkAngleX = 0.6f;
        //if (point.y < _aimPoint.position.y && point.x < checkAngleX && point.x > -checkAngleX)
        //{
        //    float minLength = 0.5f;
        //    float length =  minLength * Mathf.Abs(point.x) / minLength;
        //    if (length < minLength)  length = minLength;
        //    _dragPosition = _aimPoint.position + Vector3.ClampMagnitude(point, length);
        //}
        //else
        //{
        //    _dragPosition = _aimPoint.position + Vector3.ClampMagnitude(point, _maxBandLength);
        //}
        
        //_dragPosition = _aimPoint.position + Vector3.ClampMagnitude(point, _maxBandLength);
        //dragPos = _dragPosition;
        //연출
        //=====================================================================

        //밴드 끝 부분 에임 포인트 회전
        Vector2 dir = (_aimPoint.position - point);
        
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //Quaternion axis = Quaternion.AngleAxis(angle , Vector3.forward);
        //_imgAimBand.transform.rotation = axis;
        //=====================================================================

        _imgAimBand.transform.position = point;

        _leftBandLine.SetPosition(1, point);
        _rightBandLine.SetPosition(1, point);

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
    private void FixedUpdate()
    {
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
            Shoot();
        }


    }


}
