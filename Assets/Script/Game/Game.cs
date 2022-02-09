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

    private void Awake()
    {
        InputManager.Instance.touchEvent += OnTouch;
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
        //Map Seed
        Random.InitState(randomSeed);

        _mapIDList.Add(56);
        _mapIDList.Add(Random.Range(100, 200));
        _mapIDList.Add(Random.Range(300, 500));

        //MapLoad �Ϸ� üũ - �� ������Ʈ ���� �س���
        _board.Init(_mapIDList.ToArray(),
            ()=> 
            {
                //LoadingUI ����
            });

    }

    /// <summary>
    /// ������ ���� 
    /// </summary>
    /// <param name="index"></param>
    public void Reload(int index)
    {
        

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


    private void OnTouch(TouchPhase phase, int id,float x, float y, float dx, float dy)
    {

        switch (phase)
        {
            case TouchPhase.Began:
                
                _board.ReadyBrick();
                _board.CheckShootReady(new Vector2(x, y));
                break;
            case TouchPhase.Moved:
                _board.SetAiming(new Vector2(x, y));

                break;
            case TouchPhase.Ended:
                break;
        }

    }








}
