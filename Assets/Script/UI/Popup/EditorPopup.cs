using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorPopup : MonoBehaviour
{

    public void PlayButton(int leage)
    {
        Game.I.Shuffle((Game.LEAGUE)leage, Random.Range(0, int.MaxValue));
        gameObject.SetActive(false);

    }

}
