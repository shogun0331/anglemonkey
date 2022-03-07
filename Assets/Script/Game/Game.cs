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

    // �ʿ� ���� ������ ���� 
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
    [SerializeField] int[] _easy;
    [SerializeField] int[] _nomal;
    [SerializeField] int[] _hard;


    public enum LEAGUE
    {
        CASH_1 = 0,
        CASH_2,
        CASH_3,
        Z1,
        Z6,
        Z30,
        Z60
    }

    public enum SCORE_TYPE
    {
        BANANA = 0,
        TOKKEN,
        DESTROY,
        MONKEY,
        TIME,
        ClEAR
    }

    Dictionary<SCORE_TYPE, int> _dicScore = new Dictionary<SCORE_TYPE, int>();

    private void Awake()
    {
        _dicScore.Clear();
        InputManager.Instance.touchEvent += OnTouch;


    }

    public void SetLevel(int[] easy, int[] nomal, int[] hard)
    {
        _easy = easy;
        _nomal = nomal;
        _hard = hard;
    }

    /// <summary>
    /// ���׺� �� ����
    /// </summary>
    /// <param name="type">���� Ÿ��</param>
    /// <param name="seed">���� �õ�</param>
    public void Shuffle(LEAGUE type, int seed)
    {
        _isGameOver = false;

        //�õ� �ʱ�ȭ
        Random.InitState(seed);

        int[] easy = _easy;
        int[] nomal = _nomal;
        int[] hard = _hard;

        Etc.ShuffleArray(easy);
        Etc.ShuffleArray(nomal);
        Etc.ShuffleArray(hard);

        int[] mapIds = new int[3];

        switch (type)
        {
            case LEAGUE.CASH_1:
                // e - e - n
                mapIds[0] = easy[0];
                mapIds[1] = easy[1];
                mapIds[2] = nomal[2];
                break;

            case LEAGUE.CASH_2:
                //e - n - n
                mapIds[0] = easy[0];
                mapIds[1] = nomal[1];
                mapIds[2] = nomal[2];
                break;

            case LEAGUE.CASH_3:
                //n - n - n
                mapIds[0] = nomal[0];
                mapIds[1] = nomal[1];
                mapIds[2] = nomal[2];
                break;

            case LEAGUE.Z1:
                //e - e - e
                mapIds[0] = easy[0];
                mapIds[1] = easy[1];
                mapIds[2] = easy[2];
                break;

            case LEAGUE.Z6:
                //e - e - n
                mapIds[0] = easy[0];
                mapIds[1] = easy[1];
                mapIds[2] = nomal[2];
                break;

            case LEAGUE.Z30:
                //n -  e  - h
                mapIds[0] = nomal[0];
                mapIds[1] = easy[1];
                mapIds[2] = hard[2];
                break;

            case LEAGUE.Z60:
                //n - h - h
                mapIds[0] = nomal[0];
                mapIds[1] = hard[1];
                mapIds[2] = hard[2];
                break;
        }

        Load(mapIds);

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
    /// ���尡 �غ�Ǳ� �������� �ı� ���� ����
    /// </summary>
    /// <returns>�غ����</returns>
    public bool GetReady()
    {
        return _board.IsReady;
    }


    public void AddScore(Vector2 position, int score, SCORE_TYPE type)
    {

        if (_dicScore.ContainsKey(type))
            _dicScore[type] += score;
        else
            _dicScore.Add(type, score);



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

            SoundManager.Instance.Play(SoundManager.SOUND_TRACK.CLEAR);

            //Ŭ���� ���ʽ� 
            AddScore(Vector3.zero, Def.CLEAR_SCORE,SCORE_TYPE.ClEAR);

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

                AddScore(Vector2.zero, timeScore,SCORE_TYPE.TIME);

                int tmpScore = 0;

                tmpScore += _invenMonkeys[0] * Def.MONKEY1_SCORE;
                tmpScore += _invenMonkeys[1] * Def.MONKEY2_SCORE;
                tmpScore += _invenMonkeys[2] * Def.MONKEY3_SCORE;
                tmpScore += _invenMonkeys[3] * Def.MONKEY4_SCORE;
                tmpScore += _invenMonkeys[4] * Def.MONKEY5_SCORE;

                AddScore(Vector2.zero, tmpScore,SCORE_TYPE.MONKEY);

                result.Init(new int[]{
                    _dicScore[SCORE_TYPE.ClEAR],
                    _dicScore[SCORE_TYPE.BANANA],
                    _dicScore[SCORE_TYPE.TOKKEN],
                    _dicScore[SCORE_TYPE.DESTROY],
                    _dicScore[SCORE_TYPE.MONKEY],
                    _dicScore[SCORE_TYPE.TIME],
                    GameScore
                    });

                // result.Init(span.ToString(@"mm\:ss"), GameScore);

            }
        }

    }

    bool _isGameOver = false;

    private void Update()
    {
        if (!_isGameOver)
            _gameUI.SetTimer((int)GameTimer);

        if (!_isGameOver)
        {
            if (GameTimer < 0.0f)
                _isGameOver = true;

        }



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

        //MapLoad �Ϸ� üũ - �� ������Ʈ ���� �س���
        _board.Init(_mapIDList.ToArray(),
            () =>
            {
                _loading.CloseLoading(0.5f);

                StartCoroutine(startAction());
            });
    }


    public void SetResolution()
    {
        Debug.Log("width : " + Screen.width);
        Debug.Log("height : " + Screen.height);
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
        //ī�޶� ��ġ ������ ����
        _board.SetCam();
        //SetResolution();
        //SetResolution();

        yield return new WaitForSeconds(1.0f);

        //���ӽ��� �׼�
        _board.PlayAction();


        yield return new WaitForSeconds(1.0f);

        List<int> list = _board.GetMonkeyList(_mapIndex);


        for (int i = 0; i < list.Count; ++i)
        {
            AddMonkey(list[i]);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.2f);
        if (_targetIndex < 0)
            ChoiseCard(0);
    }

    public void AddMonkey(int monkeyID)
    {
        //_monkeyList.Add(monkeyID);
        //_gameUI.AddMonkey(monkeyID);

    }

    public void ButtonPause()
    {
        UIManager.Instance.showPopup(Def.POPUP_PAUSE);
    }


    public void ShootReady()
    {
        if (_board.GetBullet() == null)
            ChoiseCard(0);
    }
    public void ChoiseCard(int index)
    {
        _gameUI.ChiseCard(index);
    }

    /// <summary>
    /// ������ ���� 
    /// </summary>
    /// <param name="index"></param>
    public void Reload(int index)
    {
        if (_board.SetReload(index))
            _gameUI.SetMonkeyInfo(index);
    }

    public void TunStart()
    {

    }



    /// <summary>
    /// �� ���� - ���� �� ��ü�� �ִ��� üũ 
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


    private void OnTouch(TouchPhase phase, int id, float x, float y, float dx, float dy)
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
                _board.MonkeySkill();
                break;

            case TouchPhase.Ended:


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
