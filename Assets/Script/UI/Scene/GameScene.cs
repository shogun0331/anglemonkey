using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : UIScreen
{
    private void Awake()
    {
        UIManager.Instance.registScreen(Def.SCENE_GAME, this);
    }


    
}
