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


    private void Awake()
    {
        InputManager.Instance.touchEvent += OnTouch;
    }


    private void Start()
    {
        Load(0);
    }

    public void Load(int randomSeed)
    {
        //Map Seed
        Random.InitState(randomSeed);

        _mapIDList.Add(450);
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
