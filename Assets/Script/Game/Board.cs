using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("MapTool")]
    [SerializeField] MapTool _mapTool = null;

    List<GameObject> _brickList = new List<GameObject>();
    List<GameObject> _bananaList = new List<GameObject>();
    Shooter _shooter = null;
    public bool IsReady { get { return _isReady; } }
    
    private bool _isReady = false;
    GameObject _bullet;


    public enum State { None = 0, Shoot, Ready , LoadMap,LoadMapComplete,StartAction,Aiming }
    public State BoardState = State.None;



    public bool CompareBanana(int cnt)
    {
        return cnt >= _bananaList.Count;
        
    }


    public void Init(int[] mapIds, System.Action success)
    {
        if (_isReady) return;
        if (mapIds.Length != 3) return;


   

        BoardState = State.LoadMap;
        //3개의맵 로드  - 오브젝트 준비 - 최초 맵 재 로드  
        _mapTool.Load(mapIds[2], (result1) =>
         {
             _mapTool.Load(mapIds[1], (result2) =>
             {
                 _mapTool.Load(mapIds[0], (result3) =>
                 {
                     _brickList.Clear();
                     _bananaList.Clear();
                     _shooter = null;

                     GameObject[] objs = _mapTool.GameObjectList;

                     for (int i = 0; i < objs.Length; ++i)
                     {
                         if (objs[i].GetComponent<Brick>() != null)
                         {
                             if (objs[i].GetComponent<Rigidbody2D>() != null)
                                 objs[i].GetComponent<Rigidbody2D>().isKinematic = true;

                             _brickList.Add(objs[i]);
                             objs[i].SetActive(false);
                         }

                         if (objs[i].GetComponent<Banana>() != null)
                         {
                             if (objs[i].GetComponent<Rigidbody2D>() != null)
                                 objs[i].GetComponent<Rigidbody2D>().isKinematic = true;
                             _bananaList.Add(objs[i]);
                             objs[i].SetActive(false);
                         }

                         if (objs[i].GetComponent<Shooter>() != null)
                             _shooter = objs[i].GetComponent<Shooter>();

                         if(_shooter != null)
                         _shooter.Init();
                     }

                     
                     success?.Invoke();
                     BoardState = State.LoadMapComplete;

                 });
             });
         });

    }

    public void LoadMap(int mapId,System.Action success)
    {
        _isReady = false;
        BoardState = State.LoadMap;

        _mapTool.Load(mapId, (result) =>
        {
            _brickList.Clear();
            _bananaList.Clear();
            _shooter = null;

            GameObject[] objs = _mapTool.GameObjectList;

            for (int i = 0; i < objs.Length; ++i)
            {
                if (objs[i].GetComponent<Brick>() != null)
                {
                    _brickList.Add(objs[i]);
                    objs[i].SetActive(false);
                }

                if (objs[i].GetComponent<Banana>() != null)
                {
                    _bananaList.Add(objs[i]);
                    objs[i].SetActive(false);
                }

                if (objs[i].GetComponent<Shooter>() != null)
                    _shooter = objs[i].GetComponent<Shooter>();

                if (_shooter != null)
                    _shooter.Init();
            }

            success?.Invoke();
            BoardState = State.LoadMapComplete;
        });
    }

    

    public void PlayAction()
    {

        StartCoroutine(startAction());
    }

    public void SetCam()
    {
        //카메라 세팅 
        _mapTool.SetCamera();
    }




    IEnumerator startAction()
    {
        BoardState = State.StartAction;
        float time = 0.02f;
        int cnt = _brickList.Count;
        float createCnt = cnt * time;

        int brickIdx = 0;
        int bananaIdx = 0;

        yield return new WaitForEndOfFrame();


        //아래 있는 블럭부터 정렬
        _brickList.Sort(delegate (GameObject a, GameObject b)
        {
            float ay = a.transform.position.y;
            float by = b.transform.position.y;

            if (ay < by) return -1;
            else if (ay > by) return 1;
            else return 0;
        });


        while (true)
        {
            yield return new WaitForSeconds(time);

            for (int i = 0; i < createCnt; ++i)
            {
                if (brickIdx >= _brickList.Count)
                {
                    ReadyBrick();
                    yield return new WaitForSeconds(1.0f);
                   
                    _isReady = true;
                    BoardState = State.Ready;
                    yield break;
                }


                GameObject oj = loadPoolingObject(Def.PATH_EFFECT_DUST1, Def.EFFECT_DUST1);

                if(oj != null)
                    oj.transform.position = _brickList[brickIdx].transform.position;

                _brickList[brickIdx].SetActive(true);

                brickIdx++;

                if (bananaIdx < _bananaList.Count)
                {
                    _bananaList[bananaIdx].SetActive(true);
                    bananaIdx++;
                }
            }
            
        }
    }

    public void ReadyBrick()
    {

        for (int i = 0; i < _brickList.Count; ++i)
            _brickList[i].GetComponent<Rigidbody2D>().isKinematic = false;

        for (int i = 0; i < _bananaList.Count; ++i)
            _bananaList[i].GetComponent<Rigidbody2D>().isKinematic = false;

    }



    /// <summary>
    /// 슈팅 포인트에 터치가 되었는가 체크, 슈팅 가능 상태인가 체크
    /// </summary>
    /// <param name="touch">터치 위치</param>
    /// <returns>슈팅 가능</returns>
    public void CheckShootReady(Vector3 touch)
    {
        if (!_isReady) return;

        Vector3 position = touch;

        if (_shooter.state == Shooter.State.Ready)
        {
     
            float dist = Vector2.Distance(_shooter.transform.position, position);

            if (dist > 0.8f) return;
            else _shooter.state = Shooter.State.Aim;
        }
    }


    /// <summary>
    /// 원숭이 새총에 장전
    /// </summary>
    /// <param name="index"></param>
    public bool SetReload(int index)
    {
        if (!_isReady) return false;
        GameObject oj = null;

        switch (index)
        {
            case (int)Def.Monkey.Baby:
                 oj = loadPoolingObject(Def.PATH_MONKEY_BABY, Def.MONKEY_BABY);
                break;

            case (int)Def.Monkey.Gold:
                oj = loadPoolingObject(Def.PATH_MONKEY_GOLD, Def.MONKEY_GOLD);
                break;

            case (int)Def.Monkey.Papio:
                oj = loadPoolingObject(Def.PATH_MONKEY_PAPIO, Def.MONKEY_PAPIO);
                break;

            case (int)Def.Monkey.Macaca:
                oj = loadPoolingObject(Def.PATH_MONKEY_MACACA, Def.MONKEY_MACACA);
                break;

            case (int)Def.Monkey.Lemur:
                oj = loadPoolingObject(Def.PATH_MONKEY_LEMUR, Def.MONKEY_LEMUR);
                break;

            case (int)Def.Monkey.Nasalis:
                oj = loadPoolingObject(Def.PATH_MONKEY_NASALIS, Def.MONKEY_NASALIS);
                break;

            case (int)Def.Monkey.Hylobatidae:
                oj = loadPoolingObject(Def.PATH_MONKEY_HYLOBATIDAE, Def.MONKEY_HYLOBATIDAE);
                break;

            case (int)Def.Monkey.Gorilla:
                oj = loadPoolingObject(Def.PATH_MONKEY_GORILLA, Def.MONKEY_GORILLA);
                break;

        }
        
        oj.GetComponent<Monkey>().Ready();

            //장전
         _shooter.SetReady(oj);
         _bullet = oj;

        return true;
        
    }

    public void ClearBullet()
    {
        if (_shooter == null) return;
        if (_shooter.Bullet != null)
        {
            GB.ObjectPooling.I.Destroy(_shooter.Bullet);
            _shooter.Init();
        }



        _bullet = null;
    }

    private void Update()
    {
        if (_shooter != null)
        {
            //슈터에 원숭이가 변경 될때
            if (_shooter.Bullet != null)
            {
                if (_bullet != _shooter.Bullet)
                    _bullet = _shooter.Bullet;
            }
        }

        if (_bullet == null) return;
        
        if (_bullet.GetComponent<Monkey>() == null) return;

        //슛방향 쳐다보기 
        Monkey m = _bullet.GetComponent<Monkey>();
        m.UpdateShoot();
       
    }


    public void MonkeySkill()
    {
        if (_bullet == null) return;
        if (_bullet.GetComponent<Monkey>() == null) return;

        Monkey m = _bullet.GetComponent<Monkey>();

        if (_bullet.GetComponent<Monkey>().state == Monkey.State.Shoot)
            m.Skill();

    }
    

    public void UpdateBoard(float dt)
    {
        


    }

    private GameObject loadPoolingObject(string path, string key)
    {
        GameObject oj = null;
        if (GB.ObjectPooling.I.GetRemainingUses(key) > 0)
        {
            oj = GB.ObjectPooling.I.Import(key);
        }
        else
        {
            GameObject resources = null;

            if (GB.ObjectPooling.I.CheckModel(key))
            {
                resources = GB.ObjectPooling.I.GetModel(key);
                oj = Instantiate(resources);
            }
            else
            {
                resources = Resources.Load<GameObject>(path);
                GB.ObjectPooling.I.RegistModel(key, resources);
                oj = Instantiate(resources);
            }

            GB.ObjectPooling.I.Registration(key, oj, true);
        }

        return oj;
    }

    /// <summary>
    /// 새총을 조준
    /// </summary>
    /// <param name="point">터치 포인트</param>
    public void SetAiming(Vector3 touch)
    {
        if (!_isReady) return;
        if (_shooter.state != Shooter.State.Aim) return;

        Vector3 position = touch;
        _shooter.SetAming(touch);
        BoardState = State.Aiming;

    }


    /// <summary>
    /// 발사 
    /// </summary>
    /// <param name="velocity">속도</param>
    public bool Shoot()
    {
        if (!_isReady) return false;
        if (_shooter.state != Shooter.State.Aim) return false;

        
        _shooter.Shoot();


        BoardState = State.Shoot;
        return true;
    }











}
