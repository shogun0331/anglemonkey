using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : UIScreen
{
    private void Awake()
    {
        UIManager.Instance.registScreen(Def.SCENE_MAIN,this);
    }

    public void ButtonPlay()
    {
        //SkillzUI
    }
    public void ButtonOption()
    {
        UIManager.Instance.showPopup(Def.POPUP_SETTING);
    }

    
}
