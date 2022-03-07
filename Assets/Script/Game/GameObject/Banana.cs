using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    private ModelBanana _model;
    [SerializeField] int _type;

    [SerializeField] float _hp;
    [SerializeField] int _score;

    bool _isDestroy = false;
    public bool IsDestroy { get { return _isDestroy; } }


    Vector2 _orijinScale;

    public int Type
    {
        get
        {
            return _type;
        }
    }

    public void SetModel(ModelBanana banana)
    {
        _model = banana;
        _hp = _model.Hp;
        _isDestroy = false;
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!Game.I.GetReady()) return;
        float magnitude = coll.relativeVelocity.sqrMagnitude;

        Rigidbody2D rg = GetComponent<Rigidbody2D>();
        if (rg == null) return;
        if (coll.rigidbody == null) return;

        float damage = 0.0f;

        //����� �浹 �ӵ��� ������ �浹 �ӵ� �� �� ũ�Ⱑ ū������ ������ �Ҵ��Ѵ�.
        damage = Mathf.Max(coll.relativeVelocity.magnitude * coll.rigidbody.mass, rg.velocity.magnitude * rg.mass);

        float hp = _hp - damage;


        if (hp < 0.0f)
        {
            DestroyBanana();
        }
        else
            _hp -= damage;
    }

    private void DestroyBanana()
    {
        if (_isDestroy) return;
        _isDestroy = true;


        SoundManager.Instance.Play(SoundManager.SOUND_TRACK.BANANA);

        GameObject oj =  loadPoolingObject(Def.PATH_EFFECT_WOOD, Def.EFFECT_WOOD);
        oj.transform.position = transform.position;

        Game.I.DestroyBanana();
        Game.I.AddScore(transform.position, _score,Game.SCORE_TYPE.BANANA);

        Destroy(gameObject);
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
}
