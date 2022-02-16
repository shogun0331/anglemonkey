using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedGuide : MonoBehaviour
{
    public const int COLOR_BLUE = 1;
    public const int COLOR_PURPLE = 2;
    public const int COLOR_YELLOW = 3;

    public enum Color { Blue = 0, Purple, Yellow };

    List<Vector2> _listPoints = new List<Vector2>();
    GameObject[] _dots;

    private Sprite _sprBlue;
    private Sprite _sprPurple;
    private Sprite _sprYellow;

    const string PATH_DOT_BLUE   = "Image/Dotted/guide_line_01";
    const string PATH_DOT_PURPLE = "Image/Dotted/guide_line_02";
    const string PATH_DOT_YELLOW = "Image/Dotted/guide_line_03";


    const string PATH_DOTTED = "PoolingObjects/Game/Dotted/Dotted";

    bool _isInit = false;

    public bool IsReady { get { return _isReady; } }

    bool _isReady = false;
    public void Init()
    {
        _sprBlue = Resources.Load<Sprite>(PATH_DOT_BLUE);
        _sprPurple = Resources.Load<Sprite>(PATH_DOT_PURPLE);
        _sprYellow = Resources.Load<Sprite>(PATH_DOT_YELLOW);
        _isInit = true;
    }

    public void Ready(List<Vector2> points, DottedGuide.Color color,bool scaleAction = false)
    {
        if (!_isInit) Init();
        if (points.Count < 3) return;
        
        Clear();

        _dots = new GameObject[points.Count - 1];
        _listPoints = points;

        Sprite sprColor = _sprBlue;
        switch (color)
        {
            case Color.Blue:    sprColor = _sprBlue;      break;
            case Color.Purple:  sprColor = _sprPurple;  break;
            case Color.Yellow:  sprColor = _sprYellow;  break;
        }

        for (int i = 0; i < _dots.Length; ++i)
        {
   
            GameObject oj = null;

            if (GB.ObjectPooling.I.GetRemainingUses("Dotted") > 0)
            {
                oj = GB.ObjectPooling.I.Import("Dotted");
            }
            else
            {
                GameObject resources = null;

                if (GB.ObjectPooling.I.CheckModel("Dotted"))
                {
                    resources = GB.ObjectPooling.I.GetModel("Dotted");
                    oj = Instantiate(resources);
                }
                else
                {
                    resources = Resources.Load<GameObject>(PATH_DOTTED);
                    GB.ObjectPooling.I.RegistModel("Dotted", resources);
                    oj = Instantiate(resources);
                }

                GB.ObjectPooling.I.Registration("Dotted", oj, true);

            }

            if (oj == null) return;

            
            oj.transform.SetParent(transform);
            _dots[i] = oj;



            _dots[i].transform.localScale = Vector3.one;

            if (scaleAction)
            {
                float sz = i * 0.08f;
                _dots[i].transform.localScale = Vector3.one;
                Vector3 scale = _dots[i].transform.localScale;

                if (sz > 1.0f)
                {
                    scale = Vector3.zero;
                }
                else
                {
                    scale.x -= sz;
                    scale.y -= sz;
                }
                _dots[i].transform.localScale = scale;
            }

            _dots[i].transform.position = _listPoints[i];
            _dots[i].GetComponent<SpriteRenderer>().sprite = sprColor;
        }

        _isReady = true;

    }

    public void Draw(List<Vector2> points, DottedGuide.Color color, bool scaleAction = false, bool isAnimation = false)
    {
        if (!_isInit) Init();
        Clear();
        Sprite sprColor = _sprBlue;
        switch (color)
        {
            case Color.Blue: sprColor = _sprBlue; break;
            case Color.Purple: sprColor = _sprPurple; break;
            case Color.Yellow: sprColor = _sprYellow; break;
        }


    }


    public void Draw(List<Vector2> points)
    {
        if (!_isReady) return;

       _listPoints = points;

        for (int i = 0; i < _dots.Length; ++i)
            _dots[i].transform.position = _listPoints[i];
    }


    float _time;

    public void DrawAnimation(List<Vector2> points, float dt)
    {
        if (!_isReady) return;

        _time +=dt ;
        _listPoints = points;

        for (int i = 0; i < _dots.Length; ++i)
        {
            _dots[i].SetActive(true);
            _dots[i].transform.position = _listPoints[i];
            _dots[i].transform.position = Vector2.MoveTowards(_listPoints[i], _listPoints[i + 1], _time);
        }

        if (_time > 1.0f)
            _time -= 1.0f;
       

    }



    public void Clear()
    {
        if (_dots == null) return;
        _time = 0.0f;

        _isReady = false;
        

        for (int i = 0; i < _dots.Length; ++i)
            GB.ObjectPooling.I.Destroy(_dots[i]);
        
        _dots = null;
        _listPoints.Clear();

    }





    
}
