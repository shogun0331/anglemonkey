using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{



    [SerializeField] int _ID;


    [Header("Camera Point")]

    [SerializeField] Vector2 _sprSize;
    public Vector2 SpriteSize { get { return _sprSize; } }

    //왼쪽 이미지 위치
    [SerializeField] Vector2 _leftPoint;
    public Vector2 LeftPoint { get { return _leftPoint; } }

    //오른쪽 이미지 위치
    [SerializeField] Vector2 _rightPoint;
    public Vector2 RightPoint { get { return _rightPoint; } }


    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        getSpriteSize();

        _leftPoint = GetLeftPoint();
        _rightPoint = GetRightPoint();
    }



    public Vector2 GetRightPoint()
    {
        Vector2 point = Vector2.zero;
        point.x = transform.position.x + (_sprSize.x * 0.5f)  * transform.localScale.x;

        point.y = transform.position.y - (_sprSize.y * 0.5f) * transform.localScale.x;
        return point;
    }


    public Vector2 GetLeftPoint()
    {

        Vector2 point = Vector2.zero;
        point.x = transform.position.x - (_sprSize.x * 0.5f) * transform.localScale.x;
        point.y = transform.position.y + (_sprSize.y * 0.5f) * transform.localScale.x;

        return point;
    }


    private Vector2 getSpriteSize()
    {
        Vector2 sprSize = Vector2.zero;

        SpriteRenderer render = GetComponent<SpriteRenderer>();

        Sprite spr = null;

        if(render != null)
            spr = render.sprite;

        if (spr != null)
            sprSize = spr.bounds.size;
         
        int len = transform.childCount;
        
        for (int i = 0; i < len; ++i)
        {
            render = transform.GetChild(i).GetComponent<SpriteRenderer>();

            if (render != null)
                spr = render.sprite;

            if (spr != null)
            {
                if (sprSize.x < spr.bounds.size.x)
                        sprSize = spr.bounds.size;
            }
        }

        _sprSize = sprSize;

        return sprSize;

    }


    public int GetID()
    {
        return _ID;
    }




}
