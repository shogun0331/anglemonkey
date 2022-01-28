using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideLine : MonoBehaviour
{

    [SerializeField] LineRenderer _line = null;
    [SerializeField] Material _mat = null;
    Vector3[] _positions;
    private float _offset = 0.0f;

    public Vector3[] GetPositions()
    {
        return _positions;
    }


    public void SetPositions(Vector3[] positions)
    {
        _line.positionCount = positions.Length;

        float length = 0;
        for (int i = 0; i < positions.Length; ++i)
        {
            length += Mathf.Abs( positions[i].x) + Mathf.Abs(positions[i].y);
        }
        length *= 0.15f;
        _positions = positions;
        _line.SetPositions(_positions);
        _mat.mainTextureScale = new Vector2(length, _mat.mainTextureScale.y);
    }


    public void SetPositions(List<Vector3> positionList)
    {
        _line.positionCount = positionList.Count;

        float length = 0;
        for (int i = 0; i < positionList.Count; ++i)
        {
            length +=Mathf.Abs( positionList[i].x) +Mathf.Abs( positionList[i].y);
            
        }

        length *= 0.15f;
        _positions = positionList.ToArray();
        _line.SetPositions(_positions);
        _mat.mainTextureScale = new Vector2(length, _mat.mainTextureScale.y);
    }

    public void UpdateOffset(float dt)
    {
        _offset -= dt;
        _mat.mainTextureOffset = new Vector2(_offset, 0.0f);
    }

    public void Clear()
    {
        _offset = 0.0f;
        _line.positionCount = 0;
    }
         









}
