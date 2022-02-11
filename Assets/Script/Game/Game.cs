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

    private static Game _i = null;
    [SerializeField] Board _board = null;

    // �ʿ� ���� ������ ���� 
    private List<int> _invenMoney = new List<int>();

    private List<int> _mapIDList = new List<int>();

    public int GameScore = 0;
    private int _subScore = 0;

    List<GameObject> _trailDottedList = new List<GameObject>();


    [SerializeField] Loading _loading = null;

    private void Awake()
    {
        InputManager.Instance.touchEvent += OnTouch;
    }


    public void SetTrailDotted(GameObject obj)
    {
        _trailDottedList.Add(obj);
    }

    /// <summary>
    /// ���尡 �غ�Ǳ� �������� �ı� ���� ����
    /// </summary>
    /// <returns>�غ����</returns>
    public bool GetReady()
    {
        return _board.IsReady;
    }



    private void Start()
    {
        GameScore = 0;
        Load(0);
    }

    public void AddScore(int score)
    {
        GameScore += score;
        _subScore -= score;
    }

    public bool CheckScore()
    {
        return GameScore == Mathf.Abs(_subScore);
    }
    

    public void Load(int randomSeed)
    {
        _loading.Show();

        //Map Seed
        Random.InitState(randomSeed);

        _mapIDList.Add(350);
        _mapIDList.Add(Random.Range(100, 200));
        _mapIDList.Add(Random.Range(300, 500));

        //MapLoad �Ϸ� üũ - �� ������Ʈ ���� �س���
        _board.Init(_mapIDList.ToArray(),
            ()=> 
            {
                _loading.CloseLoading(0.5f);
                StartCoroutine(startAction());
            });
    }
    
    

    IEnumerator startAction()
    {
        yield return new WaitForEndOfFrame();
        //ī�޶� ��ġ ������ ����
        _board.SetCam();
        yield return new WaitForSeconds(0.5f);
        //���ӽ��� �׼�
        _board.PlayAction();
    }

    /// <summary>
    /// ������ ���� 
    /// </summary>
    /// <param name="index"></param>
    public void Reload(int index)
    {
        _board.SetReload(index);
        

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
                    clearTrajectory();


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
