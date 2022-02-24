using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    private int _resultTime;


    private int[] _invenMonkeys = new int[5];


    [Header("Map Level")]
    [SerializeField] List<int> _easyList = new List<int>();
    [SerializeField] List<int> _nomalList = new List<int>();
    [SerializeField] List<int> _hardList = new List<int>();


    private void Awake()
    {
        InputManager.Instance.touchEvent += OnTouch;
    }

    public void SetLevel(List<int> easy, List<int> nomal, List<int> hard)
    {
        _easyList = easy;
        _nomalList = nomal;
        _hardList = hard;

    }

    public void SetTrailDotted(GameObject obj)
    {
        _trailDottedList.Add(obj);
    }


    public void SetTargetIndex(int index)
    {
        _targetIndex = index;
    }
    private void Start()
    {
        //GameScore = 0;
        //Load(0);

        _gameUI.SetScore(GameScore);
    }



    /// <summary>
    /// 보드가 준비되기 전까지는 파괴 되지 않음
    /// </summary>
    /// <returns>준비상태</returns>
    public bool GetReady()
    {
        return _board.IsReady;
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
            AddScore(Vector3.zero, Def.CLEAR_SCORE);

            _mapIndex++;

            if (_mapIndex < _mapIDList.Count)
            {
                clearTrajectory();

                _bananaDestroyCnt = 0;
                StartCoroutine(loadDelayLoadMap(2.0f));
            }
            else
            {
                UIManager.Instance.showPopup(Def.POPUP_RESULT);
                ResultPopup result = UIManager.Instance.findScreen(Def.POPUP_RESULT).GetComponent<ResultPopup>();

                System.TimeSpan span = new System.TimeSpan(0, 0, (int)GameTimer);

                int timeScore = (int)GameTimer;
                if (timeScore < 0) timeScore = 0;

                AddScore(Vector2.zero, timeScore);
                result.Init(span.ToString(@"mm\:ss"), GameScore);

            }
        }
        
    }

    private void Update()
    {
        _gameUI.SetTimer((int)GameTimer);
        

    }

    public void Load(int[] maps)
    {

        _mapIndex = 0;
        _loading.Show();
        _gameUI.SetStage(_mapIndex);
        _bananaDestroyCnt = 0;

        _mapIDList.Clear();
        _mapIDList.Add(maps[0]);
        _mapIDList.Add(maps[1]);
        _mapIDList.Add(maps[2]);

        for (int i = 0; i < _invenMonkeys.Length; ++i)
            _invenMonkeys[i] = 6;





        _gameStartTime = Time.time;



        _gameUI.SetTimer((int)GameTimer);
        _board.Init(_mapIDList.ToArray(),
        () =>
        {
            _loading.CloseLoading(0.5f);
            StartCoroutine(startAction());
        });




    }

    public void Load(int randomSeed)
    {

        _mapIndex = 0;
        _loading.Show();
        _gameUI.SetStage(_mapIndex);
        
        //Map Seed
        UnityEngine.Random.InitState(randomSeed);

        //_mapIDList.Add(350);
        //_mapIDList.Add(Random.Range(100, 200));
        //_mapIDList.Add(Random.Range(300, 500));

        _mapIDList.Add(350);
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
        yield return new WaitForSeconds(1.0f);

        //게임시작 액션
        _board.PlayAction();
        

        yield return new WaitForSeconds(1.0f);

        List<int> list = _board.GetMonkeyList(_mapIndex);
        

        for (int i = 0; i < list.Count; ++i)
        {
            AddMonkey(list[i]);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.2f);
        if(_targetIndex <0)
        ChoiseCard(0);
    }

    public void AddMonkey(int monkeyID)
    {
        //_monkeyList.Add(monkeyID);
        //_gameUI.AddMonkey(monkeyID);



    }




    public void ShootReady()
    {
        if(_board.GetBullet() == null)
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

                    //_gameUI.DeleteMonkey(_targetIndex);
                    //_board.UseMonkey(_mapIndex, _targetIndex);
                    UseMonkey(_targetIndex);
                    _targetIndex = -1;
                    clearTrajectory();
                }

                break;
        }

    }

    public void ResetScene()
    {
        InputManager.Instance.touchEvent -= OnTouch;
        UIManager.Instance.changeScene("Game");
    }

    public void UseMonkey(int index)
    {
        
        _invenMonkeys[index]--;
        


        _gameUI.UseMonkeyText(index, _invenMonkeys[index]);

        if (_invenMonkeys[index] <= 0)
            _gameUI.ActiveMonkey(index, false);
        
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
