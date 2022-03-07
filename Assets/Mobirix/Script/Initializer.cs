using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mobirix.GDPR;

public class Initializer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(play());
    }

    IEnumerator play()
    {
        yield return new WaitForSeconds(3.0f);
        MobirixSceneLoad();
    }
    private void MobirixSceneLoad()
    {
        if (GdprManager.Instance.CheckGdprOn())
        {
            GdprManager.Instance.LoadGdprScene();
        }
        else
        {
            UIManager.Instance.changeScene(Def.SCENE_MAIN);
        }
        
        //   if (Mobirix.GDPR.GdprManager.Instance.isGdprIpChecking)
        //     {
        //         UIManager.Instance.changeScene(ConstantData.SCENE_GDPR);
        //     }
        //     else if (GdprManager.Instance.isGdprPopOn)
        //     {
        //         UIManager.Instance.changeScene(ConstantData.SCENE_GDPR);
        //     }

    }




}
