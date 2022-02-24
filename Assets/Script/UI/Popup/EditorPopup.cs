using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorPopup : MonoBehaviour
{
    [SerializeField] InputField _inputStage1 = null;
    [SerializeField] InputField _inputStage2 = null;
    [SerializeField] InputField _inputStage3 = null;


    public void PlayButton()
    {

        int[] stageArr = new int[3];
        int.TryParse(_inputStage1.text,out stageArr[0]);
        if (!compareMinMax(stageArr[0])) return;

        int.TryParse(_inputStage2.text, out stageArr[1]);
        if (!compareMinMax(stageArr[1])) return;

        int.TryParse(_inputStage3.text, out stageArr[2]);
        if (!compareMinMax(stageArr[2])) return;

        Game.I.Load(stageArr);
        gameObject.SetActive(false);

    }

    bool compareMinMax(int stage)
    {
        int min = 1;
        int max = 550;

        if (stage < min) return false;
        if (stage > max) return false;

        return true;


    }

}
