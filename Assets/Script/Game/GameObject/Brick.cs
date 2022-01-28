using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private ModelBrick _model;
    public enum Shape {Rectangle1 = 0, Rectangle2, Rectangle3, Rectangle4, Triangle,Righttriangle, Square, Circle , QCircle ,End};
    [SerializeField] Shape _shape;
    public Shape MyShape{ get{ return _shape;}}
    public enum OutsideTexture { Ice = 0, Iron,Stone,Wood,End};
    [SerializeField] OutsideTexture _texture;
    public OutsideTexture MyTexture{get{ return _texture;}}

    [SerializeField] float _hp;
    [SerializeField] int _score;

    public void SetModel(ModelBrick brick)
    {
        _model = brick;
    }

    /// <summary>
    /// ���� �� JSON ������ ���������� ������ ��� ������ �����Ͷ� ��
    /// </summary>
    /// <returns>������ ����</returns>
    public bool CheckMemoryHack()
    {
        if (_model.Hp != _hp)                                                                                   return true;
        if (_model.Score != _score)                                                                         return true;
        if (_model.Mass != GetComponent<Rigidbody2D>().mass)                        return true;
        if (_model.GravityScale != GetComponent<Rigidbody2D>().gravityScale) return true;
        if (_model.AngularDrag != GetComponent<Rigidbody2D>().angularDrag) return true;

        return  false;
    }






}
