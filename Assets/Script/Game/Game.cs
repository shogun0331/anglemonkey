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

    // 맵에 따른 원숭이 갯수 
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

        //MapLoad 완료 체크 - 맵 오브젝트 생성 해놓기
        _board.Init(_mapIDList.ToArray(),
            ()=> 
            {
                //LoadingUI 끄기
            });

    }

    /// <summary>
    /// 원숭이 장전 
    /// </summary>
    /// <param name="index"></param>
    public void Reload(int index)
    {
        

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
