using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour
{

    [SerializeField] Def.Monkey _type;
    [SerializeField] GameObject _frame;
    [SerializeField] GameObject _monkey;
    [SerializeField] GameObject _shooter = null;
    [SerializeField] GameObject _back;

    public int BtnIndex { get { return _btnIndex; } }
    [SerializeField] int _btnIndex = 0;

    int _aniIndex = -1;
    bool _front = false;
    Vector2 _startPosition;

    private void Start()
    {
        _startPosition = _monkey.transform.localPosition;

        if (_frame.GetComponent<Button>() == null)
            _frame.AddComponent<Button>();

        _frame.GetComponent<Button>().onClick.AddListener(Click);
    }

    public void Init()
    {
        _monkey.transform.localPosition = _startPosition;
        Close();
    }

    public void Open()
    {
        _front = true;
        GetComponent<Animation>().Play("Card_Front");
    }

    public void Close()
    {
        _front = false;
        _back.SetActive(true);
        _frame.SetActive(false);
        _shooter.SetActive(false);
    }

    public void SetBtnIndex(int index)
    {
        _btnIndex = index;
    }
    public void SetActiveButton(bool isActive)
    {
        _frame.GetComponent<Button>().enabled = isActive;
    }

    public void Idle()
    {
        if (_aniIndex == 0) return;
        _aniIndex = 0;
        _monkey.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "idle_01", true);
        GetComponent<Animation>().Play("Card_Idle");
        _shooter.SetActive(false);
    }

    public void Shoot()
    {
        if (_aniIndex == 1) return;
        _aniIndex = 1;
        _monkey.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "wing_02", false);
        GetComponent<Animation>().Play("Card_Shoot");
        _shooter.SetActive(true);
        
    }

    public void Click()
    {
        if (!Game.I.GetReady()) return;
        Game.I.Reload((int)_type);
        Game.I.SetTargetIndex(BtnIndex);
        //transform.parent.GetComponent<CardControl>().SelectCard(BtnIndex);
    }

    public void PlayDestroy()
    {
        StartCoroutine(playDestroy());
    }

    IEnumerator playDestroy()
    {
        yield return new WaitForSeconds(2.0f);

        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
        transform.rotation = Quaternion.identity;
        _monkey.transform.localPosition = _startPosition;
        GB.ObjectPooling.I.Destroy(gameObject);

    }

}
