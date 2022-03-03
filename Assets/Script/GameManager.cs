using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum Mode
    {
        CI = 0 ,
        Agree,
        Gdpr,
        Main,
        Loading,
        GameLoading,
        GameStart,
        GamePause,
        Game,
        GameOver,
        End
    }
    public Mode mode = Mode.CI;

    private void Awake()
    {
        ChangeMode(Mode.CI);
    }

    public void ChangeMode(Mode mode)
    {

        if (mode == this.mode) return;
        this.mode = mode;

        switch (mode)
        {
            case Mode.CI:
                break;

            case Mode.Agree:
                break;

            case Mode.Gdpr:
                break;

            case Mode.Main:
                break;

            case Mode.Game:
                break;

            case Mode.Loading:
                break;

            case Mode.GameLoading:
                break;

            case Mode.GamePause:
                break;

            case Mode.GameStart:
                break;

            case Mode.GameOver:
                break;

        }

    }
 
}
