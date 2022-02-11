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
    
    
    public void Init(int[] mapIds, System.Action success)
    {
        if (_isReady) return;
        if (mapIds.Length != 3) return;


        BoardState = State.LoadMap;
        //3���Ǹ� �ε�  - ������Ʈ �غ� - ���� �� �� �ε�  
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
        //ī�޶� ���� 
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



        //�Ʒ� �ִ� ������ ����
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
    /// ���� ����Ʈ�� ��ġ�� �Ǿ��°� üũ, ���� ���� �����ΰ� üũ
    /// </summary>
    /// <param name="touch">��ġ ��ġ</param>
    /// <returns>���� ����</returns>
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
    /// ������ ���ѿ� ����
    /// </summary>
    /// <param name="index"></param>
    public void SetReload(int index)
    {
        if (!_isReady) return;
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

        }

        oj.GetComponent<Monkey>().Ready();

            //����
         _shooter.SetReady(oj);
         _bullet = oj;
        
    }

    private void Update()
    {
        if (_shooter != null)
        {
            //���Ϳ� �����̰� ���� �ɶ�
            if (_shooter.Bullet != null)
            {
                if (_bullet != _shooter.Bullet)
                    _bullet = _shooter.Bullet;
            }
        }

        if (_bullet == null) return;
        
        if (_bullet.GetComponent<Monkey>() == null) return;

        //������ �Ĵٺ��� 
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
            GameObject resources = Resources.Load<GameObject>(path);
            oj = Instantiate(resources);
            GB.ObjectPooling.I.Registration(key, oj, true);
        }

        return oj;
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <param name="point">��ġ ����Ʈ</param>
    public void SetAiming(Vector3 touch)
    {
        if (!_isReady) return;
        if (_shooter.state != Shooter.State.Aim) return;

        Vector3 position = touch;
        _shooter.SetAming(touch);
        BoardState = State.Aiming;

    }


    /// <summary>
    /// �߻� 
    /// </summary>
    /// <param name="velocity">�ӵ�</param>
    public bool Shoot()
    {
        if (!_isReady) return false;
        if (_shooter.state != Shooter.State.Aim) return false;
        _shooter.Shoot();


        BoardState = State.Shoot;
        return true;
    }











}
