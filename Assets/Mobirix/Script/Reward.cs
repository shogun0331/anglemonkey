using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Mobirix
{
    public class Reward : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            
        }

        /// <summary>
        /// Reward 서버에 접속
        /// </summary>
        /// <param name="go"> 함수를 호출하는 게임오브젝트 </param>
        /// <param name="gameid"> 해당 게임의 게임ID </param>
        /// <param name="uuid"> 12자리 플레이어 UUID </param>
        /// <param name="callback"> 요청 결과 콜백 함수 </param>
        public static void Connect(GameObject go, string gameid, string uuid, System.Action<string> callback)
        {
            // 코루틴을 실행 할 수 없기 때문에 리턴
            if (go == null)
                return;

            Reward reward = go.GetComponent<Reward>();

            // 이미 리워드 보상 진행 중인 경우 
            if (reward != null)
                return;

            reward = go.AddComponent<Reward>();

            reward.StartCoroutine(reward.getReward(uuid, gameid, callback));
        }

        IEnumerator getReward(string uuid, string gameid, System.Action<string> callback)
        {
            WWWForm form = new WWWForm();
            form.AddField("idx", uuid);
            form.AddField("gameid", gameid);
            using (UnityWebRequest www = UnityWebRequest.Post("http://uu22rr33iuerwol0ciure.kr:33364/MobirixGameData/GameReward.aspx", form))
            {
                www.timeout = 5;

                yield return www.SendWebRequest();

                // check error
                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogErrorFormat("connection error : {0}", www.error);
                    Destroy(this);
                    yield break;
                }

                // get reward
                string reward = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);

                if (callback != null)
                    callback(reward);

            }

            Destroy(this);
        }
    }
}