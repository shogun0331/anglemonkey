using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTool : MonoBehaviour
{
    //재질   ,  모양 순서대로  
    Dictionary<int, List<Brick>> _dicBrickModels = new Dictionary<int, List<Brick>>();
    Dictionary<int, Ground> _dicGroundModels = new Dictionary<int, Ground>();
    [SerializeField] List<GameObject> _gameObjectList = new List<GameObject>();

    Transform _brickGroup;
    Transform _groundGroup;
    Transform _monkeyGroup;
    Transform _bananaGroup;
    Transform _springGroup;

    const string PATH_BRICK = "PoolingObjects/Game/Brick";
    const string PATH_GROUND = "PoolingObjects/Game/Ground";
    const string PATH_BANANA = "PoolingObjects/Game/Banana";
    const string PATH_SHOOTER = "PoolingObjects/Game/Shooter/Shooter";

    const string TYPE_BG = "BG";
    const string TYPE_GROUND = "Ground";
    const string TYPE_Monkey = "Monkey";
    const string TYPE_BRICK = "Brick";
    const string TYPE_BANANA = "Banana";
    const string TYPE_SPRING = "Spring";
    const string TYPE_SHOOTER = "Shooter";
    const string TYPE_HINGE = "HingeJoint2D";

    public Vector2 MapScale;

    private int _mapID = 0;

    private void Start()
    {
        Load(1);
    }

    public void Load(int mapId)
    {
        _mapID = mapId;
        _dicBrickModels.Clear();
        _dicGroundModels.Clear();

        if(_brickGroup == null)
        _brickGroup = new GameObject("Bricks").transform;
        if (_groundGroup == null)
            _groundGroup = new GameObject("Grounds").transform;
        if (_monkeyGroup == null)
            _monkeyGroup = new GameObject("Monkeys").transform;
        if (_bananaGroup == null)
            _bananaGroup = new GameObject("Bananas").transform;
        if (_springGroup == null)
            _springGroup = new GameObject("Springs").transform;

        _brickGroup.SetParent(transform);
        _groundGroup.SetParent(transform);
        _monkeyGroup.SetParent(transform);
        _bananaGroup.SetParent(transform);
        _springGroup.SetParent(transform);

        GameObject[] brickModels = Resources.LoadAll<GameObject>(PATH_BRICK);
        GameObject[] groundModels = Resources.LoadAll<GameObject>(PATH_GROUND);
        GameObject[] bananaModels = Resources.LoadAll<GameObject>(PATH_BANANA);
        

        for (int i = 0; i < groundModels.Length; ++i)
        {
            Ground ground = groundModels[i].GetComponent<Ground>();
            _dicGroundModels.Add(ground.GetID(), ground);
        }


        //  =====================================
        //                브릭  재질별로 모음
        //  =====================================
        for (int i = 0; i < brickModels.Length; ++i)
        {
            Brick brick = brickModels[i].GetComponent<Brick>();

            if (brick == null) break;
            int texture = (int)brick.MyTexture;

            if (_dicBrickModels.ContainsKey(texture))
            {
                _dicBrickModels[texture].Add(brick);
            }
            else
            {
                List<Brick> list =  new List<Brick>();
                list.Add(brick);
                _dicBrickModels.Add(texture, list);
            }
        }

        //  =====================================
        //          브릭    모양 순서대로 정렬
        //  =====================================
        int count = (int)Brick.OutsideTexture.End;

        for (int i = 0; i < count; ++i)
        {
            _dicBrickModels[i].Sort(delegate (Brick x, Brick y)
             {
                 int a = (int)x.MyShape;
                 int b = (int)y.MyShape;

                 if (a < b) return -1;
                 else if (a > b) return 1;
                 else return 0;
            });
        }
        
        //  =====================================
        //              맵파일 가져와서 JSON Load
        //  =====================================
        string path = "Map/" + mapId;
        string data = Resources.Load<TextAsset>(path).text;
        Items items = JsonUtility.FromJson<Items>(data);

        List<ModelHinge> hingeList = new List<ModelHinge>();

        for (int i = 0; i < items.items.Length; ++i)
        {
            Item item = items.items[i];

            Vector3 position;

            Vector3 scale;

            Quaternion rotation;
            GameObject oj;

            switch (item.Type)
            {
                case TYPE_BRICK:
                    ModelBrick mBrick = JsonUtility.FromJson<ModelBrick>(item.Json);
                    position = PaserVec3(item.Position);
                    rotation = Quaternion.Euler(PaserVec3(item.Rotation));
                    scale = PaserVec3(item.Scale);

                    //질감
                    int texture = -1;
                    if (string.Equals(mBrick.Tag, "IceObject"))
                        texture = (int)Brick.OutsideTexture.Ice;
                    else if (string.Equals(mBrick.Tag, "WoodObject"))
                        texture = (int)Brick.OutsideTexture.Wood;
                    else if (string.Equals(mBrick.Tag, "StoneObject"))
                        texture = (int)Brick.OutsideTexture.Stone;
                    else if (string.Equals(mBrick.Tag, "IronObject"))
                        texture = (int)Brick.OutsideTexture.Iron;


                    if (GB.ObjectPooling.I.GetRemainingUses(_dicBrickModels[texture][mBrick.Type].name) > 0)
                    {

                        oj = GB.ObjectPooling.I.Import(_dicBrickModels[texture][mBrick.Type].name);
                    }
                    else
                    {
                        oj = Instantiate(_dicBrickModels[texture][mBrick.Type].gameObject, _groundGroup); ;
                        GB.ObjectPooling.I.Registration(_dicBrickModels[texture][mBrick.Type].name, oj,true);
                    }

                    oj.transform.SetParent(_brickGroup);
                    oj.transform.position = position;
                    oj.transform.rotation = rotation;
                    oj.transform.localScale = scale;
                    if (oj.GetComponent<Rigidbody2D>() != null)
                        oj.GetComponent<Rigidbody2D>().isKinematic = true;

                    _gameObjectList.Add(oj);
                    break;

                case TYPE_BG:
                    break;
                case TYPE_GROUND:
                    position = PaserVec3(item.Position);
                    rotation = Quaternion.Euler(PaserVec3(item.Rotation));
                    scale = PaserVec3(item.Scale);

                    ModelGround mGround = JsonUtility.FromJson<ModelGround>(item.Json);

                    if (GB.ObjectPooling.I.GetRemainingUses(_dicGroundModels[mGround.Type].name) > 0)
                    {
                        oj = GB.ObjectPooling.I.Import(_dicGroundModels[mGround.Type].name);
                    }
                    else
                    {
                        oj = Instantiate(_dicGroundModels[mGround.Type].gameObject, _groundGroup);
                        GB.ObjectPooling.I.Registration(_dicGroundModels[mGround.Type].gameObject.name, oj,true);
                    }

                    
                    oj.transform.SetParent(_groundGroup);
                    oj.transform.position = position;
                    oj.transform.rotation = rotation;
                    oj.transform.localScale = scale;
                    _gameObjectList.Add(oj);
                    break;
                case TYPE_Monkey:
                    break;
                case TYPE_SPRING:
                    break;
                case TYPE_BANANA:
                    position = PaserVec3(item.Position);
                    rotation = Quaternion.Euler(PaserVec3(item.Rotation));
                    scale = PaserVec3(item.Scale);

                    ModelBanana  mBanana = JsonUtility.FromJson<ModelBanana>(item.Json);

                    for (int j = 0; j < bananaModels.Length; ++j)
                    {
                        if (mBanana.Type == bananaModels[j].GetComponent<Banana>().Type)
                        {

                            if (GB.ObjectPooling.I.GetRemainingUses(bananaModels[j].name) > 0)
                            {
                                oj = GB.ObjectPooling.I.Import(bananaModels[j].name);
                            }
                            else
                            {
                                oj = Instantiate(bananaModels[j].gameObject);
                                GB.ObjectPooling.I.Registration(bananaModels[j].gameObject.name, oj,true);
                            }
                            
                            oj.transform.SetParent(_bananaGroup);
                            oj.transform.position = position;
                            oj.transform.rotation = rotation;
                            oj.transform.localScale = scale;
                            _gameObjectList.Add(oj);
                            break;
                        }
                    }

                    break;
                case TYPE_SHOOTER:
                    position = PaserVec3(item.Position);

                    GameObject tmp = Resources.Load(PATH_SHOOTER) as GameObject;

                    if (GB.ObjectPooling.I.GetRemainingUses(tmp.name) > 0)
                    {
                        oj = GB.ObjectPooling.I.Import(tmp.name);
                    }
                    else
                    {
                        oj = Instantiate(tmp);
                        GB.ObjectPooling.I.Registration(tmp.name, oj, true);
                    }

                    oj.transform.SetParent(transform);
                    oj.transform.position = position;
                    
                    _gameObjectList.Add(oj);

                    break;

                case TYPE_HINGE:
                    ModelHinge hinge = JsonUtility.FromJson<ModelHinge>(item.Json);
                    hingeList.Add(hinge);
                    break;
            }
        }

        for (int i = 0; i < hingeList.Count; ++i)
        {
            Debug.Log("HingeJointName : " + _gameObjectList[hingeList[i].ItemID].name);
            Debug.Log("JointName : " + _gameObjectList[hingeList[i].JointID].name);

            //HingeJoint2D joint = _gameObjectList[hingeList[i].ItemID - 1].AddComponent<HingeJoint2D>();
            //joint.connectedBody = _gameObjectList[hingeList[i].JointID - 1].GetComponent<Rigidbody2D>();
            //joint.connectedAnchor = PaserVec3(hingeList[i].ConnectedAnchor);
            //joint.anchor = PaserVec3(hingeList[i].Anchor);
        }

        //  =====================================
        //              맵의 크기
        //  =====================================
        Vector2 min = new Vector2();
        Vector2 max = new Vector2();

        for (int i = 0; i < _gameObjectList.Count; ++i)
        {
            Vector2 pos = _gameObjectList[i].transform.position;

            if (pos.x < min.x)  min.x = pos.x;
            if (pos.x > max.x) max.x = pos.x;
            if (pos.y < min.y)  min.y = pos.y;
            if (pos.y > max.y) max.y = pos.y;
        }

        MapScale.x = Mathf.Abs(min.x) + Mathf.Abs(max.x);
        MapScale.y = Mathf.Abs(min.y) + Mathf.Abs(max.y);


    }

    private Vector3 PaserVec3(string data)
    {
        string[] sprit = data.Split(',');

        return new Vector3(
           float.Parse(sprit[0], System.Globalization.CultureInfo.InvariantCulture),
           float.Parse(sprit[1], System.Globalization.CultureInfo.InvariantCulture),
           float.Parse(sprit[2], System.Globalization.CultureInfo.InvariantCulture));

    }

    public void Clear()
    {

        for (int i = 0; i < _gameObjectList.Count; ++i)
            GB.ObjectPooling.I.Destroy(_gameObjectList[i]);
        
        if (_brickGroup != null) 
        Destroy(_brickGroup.gameObject);
        if (_bananaGroup != null)
            Destroy(_bananaGroup.gameObject);
        if (_groundGroup != null)
            Destroy(_groundGroup.gameObject);
        if (_springGroup != null)
            Destroy(_springGroup.gameObject);
        if (_monkeyGroup != null)
            Destroy(_monkeyGroup.gameObject);

        _gameObjectList.Clear();
        _dicBrickModels.Clear();
        _dicGroundModels.Clear();
    }

#if UNITY_EDITOR

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.fontSize = h * 2 / 40;
        style.normal.textColor = Color.red;
        string text = "Map ID : " + _mapID + "\nKey : W,A,S,D";
        GUI.Label(rect, text, style);

    }

    int _tmpMapid = 1;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (_isLoading) return;
            _tmpMapid++;
            if (_tmpMapid > 550)
                _tmpMapid = 550;
          
            StartCoroutine(LoadMap(_tmpMapid));
        }


        if (Input.GetKeyDown(KeyCode.W))
        {
            if (_isLoading) return;
            _tmpMapid+= 10;
            if (_tmpMapid > 550)
                _tmpMapid = 550;
                
            
            StartCoroutine(LoadMap(_tmpMapid));
        }



        if (Input.GetKeyDown(KeyCode.A))
        {
            if (_isLoading) return;
            _tmpMapid--;
            if (_tmpMapid < 1)
                _tmpMapid = 1;
                
            
            StartCoroutine(LoadMap(_tmpMapid));
        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_isLoading) return;
            _tmpMapid-=10;
            if (_tmpMapid < 1)
                _tmpMapid = 1;
                
            
            StartCoroutine(LoadMap(_tmpMapid));
        }

    }

    bool _isLoading = false;
    IEnumerator LoadMap(int mapid)
    {
        if (_isLoading) yield return null;
        _isLoading = true;
        Clear();
        yield return new WaitForSeconds(0.2f);
        Load(mapid);
        _isLoading = false;
    }
#endif




}
