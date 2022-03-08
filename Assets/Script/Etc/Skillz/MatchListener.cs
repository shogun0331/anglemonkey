using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillzSDK;

namespace GB
{
    public class MatchListener : MonoBehaviour, SkillzMatchDelegate
    {
        protected SkillzSDK.Player Player { get { return _player; } }
        SkillzSDK.Player _player;

        protected SkillzSDK.Player OpponentPlayer { get { return _opponentPlayer; } }
        SkillzSDK.Player _opponentPlayer;

        // Called when a player chooses a tournament and the match countdown expires
        public void OnMatchWillBegin(Match matchInfo)
        {
            for (int i = 0; i < matchInfo.Players.Count; ++i)

                if (matchInfo.Players[i].IsCurrentPlayer)
                    _player = matchInfo.Players[i];
                else
                    _opponentPlayer = matchInfo.Players[i];

            UIManager.Instance.changeScene(Def.SCENE_GAME);
        }

        //시간 감점
        
        // Called when a player clicks the Progression entry point or side menu. Explained in later steps
        public void OnProgressionRoomEnter()
        {
            //SceneManager.LoadScene(ProgressionRoomScene);
        }

        // Called when a player chooses Exit Skillz from the side menu
        public void OnSkillzWillExit()
        {
            UIManager.Instance.changeScene(Def.SCENE_MAIN);
        }


    }
}
