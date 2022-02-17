using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public static Game I
    {
        get
        {
            if (_i == null)
            {
                _i = GameObject.FindObjectOfType<Game>();

                if (_i == null)
                {
                    GameObject obj = new GameObject("Game");
                    _i = obj.AddComponent<Game>();
                }
            }

            return _i;

        }
    }

    [SerializeField] GameScene _gameUI = null;

    private static Game _i = null;
    [SerializeField] Board _board = null;

    // 맵에 따른 원숭이 갯수 
    private List<int> _invenMoney = new List<int>();
    private List<int> _mapIDList = new List<int>();

    public int GameScore = 0;
    private int _subScore = 0;

    List<GameObject> _trailDottedList = new List<GameObject>();
    [SerializeField] Loading _loading = null;

    private int _bananaDestroyCnt = 0;
    private int _mapIndex = 0;

    public float GameTimer { get { return Def.TIMER - (Time.time - _gameStartTime); } }
    private float _gameStartTime = 0.0f;

    private List<int> _monkeyList = new List<int>();

    private int _targetIndex = -1;

    private void Awake()
    {
        InputManager.Instance.touchEvent += OnTouch;
    }


    public void SetTrailDotted(GameObject obj)
    {
        _trailDottedList.Add(obj);
    }


    public void SetTargetIndex(int index)
    {
        _targetIndex = index;
    }
 

    /// <summary>
    /// 보드가 준비되기 전까지는 파괴 되지 않음
    /// </summary>
    /// <returns>준비상태</returns>
    public bool GetReady()
    {
        return _board.IsReady;
    }

    private void Start()
    {
        GameScore = 0;
        Load(0);
        
        _gameUI.SetScore(GameScore);
    }

    public void AddScore(Vector2 position, int score)
    {
        GameScore += score;
        _subScore -= score;

        _gameUI.AddScore(Camera.main.WorldToScreenPoint(position), score);
        _gameUI.SetScore(GameScore);
    }

    public bool CheckScore()
    {
        return GameScore == Mathf.Abs(_subScore);
    }

    public void DestroyBanana()
    {
        
        _bananaDestroyCnt++;

        if (_board.CompareBanana(_bananaDestroyCnt))
        {
            //클리어 보너스 
            AddScore(Vector3.zero, 1000000);

            _mapIndex++;

            if (_mapIndex < _mapIDList.Count)
            {


                clearTrajectory();

                _bananaDestroyCnt = 0;
                StartCoroutine(loadDelayLoadMap(2.0f));
            }
        }
        
    }

    public void Load(int randomSeed)
    {

        _mapIndex = 0;
        _loading.Show();
        _gameUI.SetStage(_mapIndex);
        //Map Seed
        Random.InitState(randomSeed);



        //_mapIDList.Add(350);
        //_mapIDList.Add(Random.Range(100, 200));
        //_mapIDList.Add(Random.Range(300, 500));

        _mapIDList.Add(1);
        _mapIDList.Add(2);
        _mapIDList.Add(350);

        _bananaDestroyCnt = 0;

        //MapLoad 완료 체크 - 맵 오브젝트 생성 해놓기
        _board.Init(_mapIDList.ToArray(),
            ()=> 
            {
                _loading.CloseLoading(0.5f);
                StartCoroutine(startAction());
            });
    }


    IEnumerator loadDelayLoadMap(float delay)
    {

        yield return new WaitForSeconds(delay);
        _gameUI.InitCards();

        _loading.Show();
        _board.LoadMap(_mapIDList[_mapIndex],
               () =>
               {
                   _loading.CloseLoading(0.5f);
                   _gameUI.SetStage(_mapIndex);
                   StartCoroutine(startAction());
               });
        
    }
    

    IEnumerator startAction()
    {
        
        yield return new WaitForEndOfFrame();
        //카메라 위치 사이즈 세팅
        _board.SetCam();
        yield return new WaitForSeconds(0.5f);

        //게임시작 액션
        _board.PlayAction();
        _gameStartTime = Time.time;

        yield return new WaitForSeconds(1.0f);

        _monkeyList = _board.GetMonkeyList();

        for (int i = 0; i < _monkeyList.Count; ++i)
        {
            _gameUI.AddMonkey(_monkeyList[i]);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.2f);

        ChoiseCard(0);


    }


    public void ShootReady()
    {
        ChoiseCard(0);
    }
    public void ChoiseCard(int index)
    {
        _gameUI.ChiseCard(index);
    }

    /// <summary>
    /// 원숭이 장전 
    /// </summary>
    /// <param name="index"></param>
    public void Reload(int index)
    {
        if(_board.SetReload(index))
            _gameUI.SetMonkeyInfo(index);
    }

    public void TunStart()
    {

    }



    /// <summary>
    /// 턴 종료 - 장전 할 객체가 있는지 체크 
    /// </summary>
    public void TunEnd()
    {

    }

    public void ClearCheck()
    {

    }

    private void clearTrajectory()
    {

        for (int i = 0; i < _trailDottedList.Count; ++i)
            GB.ObjectPooling.I.Destroy(_trailDottedList[i]);
        _trailDottedList.Clear();

    }


    private void OnTouch(TouchPhase phase, int id,float x, float y, float dx, float dy)
    {
        
        Vector3 touchPosition = CunverterTouchPoint(new Vector2(x, y));
        touchPosition = paserTouchToCamPoint(touchPosition);

        switch (phase)
        {

            case TouchPhase.Began:
                _board.CheckShootReady(touchPosition);
                break;

            case TouchPhase.Moved:
                _board.SetAiming(touchPosition);
                break;

            case TouchPhase.Ended:
                _board.MonkeySkill();

                if (_board.Shoot())
                {
                    _gameUI.DeleteMonkey(_targetIndex);
                    _targetIndex = -1;
                    clearTrajectory();
                }

                break;
        }

    }


    public Vector2 CunverterTouchPoint(Vector2 touch)
    {
        Vector2 hand = touch;
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        hand *= screenSize;

        return hand;
    }


    private Vector3 paserTouchToCamPoint(Vector2 touch)
    {
        Vector3 position = touch;
        position.z = 10;
        return Camera.main.ScreenToWorldPoint(position);
    }












}
