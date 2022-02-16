using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class Card : MonoBehaviour
{
    [SerializeField] Def.Monkey _type;
    [SerializeField] GameObject _frame;
    [SerializeField] GameObject _monkey;

    public int BtnIndex { get { return _btnIndex; } }
    private int _btnIndex = 0;

    private int _aniIndex = -1;


    private void Start()
    {
        
        Idle();
        if (_frame.GetComponent<Button>() == null)
            _frame.AddComponent<Button>();

        _frame.GetComponent<Button>().onClick.AddListener(Click);
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
    }

    public void Shoot()
    {
        if (_aniIndex == 1) return;
        _aniIndex = 1;
        _monkey.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "wing_02", false);
        GetComponent<Animation>().Play("Card_Shoot");
        
    }

    public void Click()
    {
        Game.I.Reload((int)_type);
        transform.parent.GetComponent<CardControl>().SelectCard(BtnIndex);
    }


}
