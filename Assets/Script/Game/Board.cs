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

    private bool _isReady = false;

    public void Init(int[] mapIds, System.Action success)
    {
        if (_isReady) return;
        if (mapIds.Length != 3) return;

        

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

                     _isReady = true;

                     StartCoroutine(startAction());
                     success?.Invoke();

                 });
             });
         });

    }

    IEnumerator startAction()
    {
        float time = 0.0f;
        int cnt = _brickList.Count;

        int brickIdx = 0;
        int bananaIdx = 0;

        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            if (brickIdx >= _brickList.Count) yield break;

            _brickList[brickIdx].SetActive(true);
            

            brickIdx++;

            if (bananaIdx < _bananaList.Count)
            {
                _bananaList[bananaIdx].SetActive(true);
                bananaIdx++;
            }

        }

    }



    /// <summary>
    /// 슈팅 포인트에 터치가 되었는가 체크, 슈팅 가능 상태인가 체크
    /// </summary>
    /// <param name="touch">터치 위치</param>
    /// <returns>슈팅 가능</returns>
    public void CheckShootReady(Vector2 touch)
    {
        if (!_isReady) return;



        Vector3 position = paserTouchToCamPoint(touch);

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
    public void SetReload(int index)
    {
        if (!_isReady) return;
        GameObject oj;

        switch (index)
        {
            case (int)Def.Monkey.Baby:
                if (GB.ObjectPooling.I.GetRemainingUses(Def.MONKEY_BABY) > 0)
                {
                    GameObject baby = GB.ObjectPooling.I.Import(Def.MONKEY_BABY);
                    _shooter.SetReady(baby);
                }
                else
                {
                    GameObject baby = Resources.Load<GameObject>(Def.PATH_MONKEY_BABY);
                    oj = Instantiate(baby);
                    _shooter.SetReady(oj);
                    GB.ObjectPooling.I.Registration(Def.MONKEY_BABY, oj, true);
                }

                break;
        }
    }

    /// <summary>
    /// 새총을 조준
    /// </summary>
    /// <param name="point">터치 포인트</param>
    public void SetAiming(Vector3 touch)
    {
        if (!_isReady) return;
        if (_shooter.state != Shooter.State.Aim) return;

        Vector3 position = paserTouchToCamPoint(touch);
        _shooter.SetAming(touch);

    }


    /// <summary>
    /// 발사 
    /// </summary>
    /// <param name="velocity">속도</param>
    public void Shoot()
    {
        if (!_isReady) return;
        if (_shooter.state != Shooter.State.Aim) return;
        _shooter.Shoot();
    }







    private Vector3 paserTouchToCamPoint(Vector2 touch)
    {
        Vector3 position = touch;
        position.z = 10;
        return Camera.main.ScreenToWorldPoint(position);
    }



}
