using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillzSDK;

using System;

namespace GB
{
    public class SkillzManager : MatchListener
    {

        private static SkillzManager _instance;
        public static SkillzManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<SkillzManager>();

                    if (_instance == null)
                        _instance = new GameObject("SkillzManager").AddComponent<SkillzManager>();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// 시작 UI 접속
        /// </summary>
        public void LaunchSkillz()
        {
            SkillzCrossPlatform.LaunchSkillz(this);
        }

        #region Score
        public bool SummitUpdateScore(int score)
        {
            if (!SkillzCrossPlatform.IsMatchInProgress()) return false;
            SkillzCrossPlatform.UpdatePlayersCurrentScore(score);
            return true;
        }

        public bool SummitUpdateScore(float score)
        {
            if (!SkillzCrossPlatform.IsMatchInProgress()) return false;
            SkillzCrossPlatform.UpdatePlayersCurrentScore(score);
            return true;
        }

        public bool SummitUpdateScore(string score)
        {
            if (!SkillzCrossPlatform.IsMatchInProgress()) return false;
            SkillzCrossPlatform.UpdatePlayersCurrentScore(score);
            return true;
        }

        public void SubmitScore(int score, Action result, Action<string> failed)
        {
            SkillzCrossPlatform.SubmitScore(score, () =>
            {
                result?.Invoke();
            },
             (error) =>
             {
                 //Log
                 failed?.Invoke(error);
             });
        }

        public void SubmitScore(float score, Action result, Action<string> failed)
        {
            SkillzCrossPlatform.SubmitScore(score, () =>
            {
                result?.Invoke();
            },
             (error) =>
             {
                 //Log
                 failed?.Invoke(error);
             });
        }

        /// <summary>
        /// 스코어 등록
        /// </summary>
        /// <param name="score"></param>
        /// <param name="result"></param>
        /// <param name="failed"></param>
        public void SubmitScore(string score,Action result,Action<string> failed)
        {
            SkillzCrossPlatform.SubmitScore(score, () =>
            {
                result?.Invoke();
             },
             (error) =>
             {
                 //Log
                 failed?.Invoke(error);
             });
        }

        public bool ReportFinalScore(int score)
        {
            if (!SkillzCrossPlatform.IsMatchInProgress()) return false;
            SkillzCrossPlatform.DisplayTournamentResultsWithScore(score);
            return true;
        }

        public bool ReportFinalScore(float score)
        {
            if (!SkillzCrossPlatform.IsMatchInProgress()) return false;
            SkillzCrossPlatform.DisplayTournamentResultsWithScore(score);
            return true;
        }

        public bool ReportFinalScore(ulong score)
        {
            if (!SkillzCrossPlatform.IsMatchInProgress()) return false;
            SkillzCrossPlatform.DisplayTournamentResultsWithScore(score);
            return true;
        }

        public bool ReportFinalScore(string score)
        {
            if (!SkillzCrossPlatform.IsMatchInProgress()) return false;
            SkillzCrossPlatform.DisplayTournamentResultsWithScore(score);
            return true;
        }


        /// <summary>
        /// 패배 처리
        /// </summary>
        public void AbortMatch()
        {
            SkillzCrossPlatform.AbortMatch();
        }

        /// <summary>
        /// 나의 재접속 까지 남은 시간
        /// </summary>
        public void CurrentGetTimeLeftForReconnection()
        {
            ulong id = (ulong)Player.ID;
            SkillzCrossPlatform.GetTimeLeftForReconnection(id);
        }

        public void OpponentGetTimeLeftForReconnection()
        {
            ulong id = (ulong)OpponentPlayer.ID;
            SkillzCrossPlatform.GetTimeLeftForReconnection(id);
        }

        /// <summary>
        /// 사용자가 Skillz 포털로 돌아갈 수 있는지 여부를 나타내는 부울 값을 반환합니다.
        /// 그런 다음 가능하면 스킬즈 포털로 돌아갑니다.
        /// 경기 중에 호출되면 점수를 제출해야 합니다.
        /// </summary>
        /// <returns>가능 여부</returns>
        public bool ReturnToSkillz()
        {
            return SkillzCrossPlatform.ReturnToSkillz();
        }

        public void AbortBotMatch(int score)
        {
            SkillzCrossPlatform.AbortBotMatch(score);
        }

        public void AbortBotMatch(ulong score)
        {
            SkillzCrossPlatform.AbortBotMatch(score);
        }

        public void AbortBotMatch(string score)
        {
            SkillzCrossPlatform.AbortBotMatch(score);
        }

        public void AbortBotMatch(float score)
        {
            SkillzCrossPlatform.AbortBotMatch(score);
        }

        /// <summary>
        /// 초 단위로 서버 시간을 두 배로 반환합니다(프로토타입, 신뢰할 수 없음).
        /// </summary>
        /// <returns></returns>
        public double GetServerTime()
        {
            return SkillzCrossPlatform.GetServerTime();
        }

        public int Random(int min,int max)
        {
            return SkillzCrossPlatform.Random.Range(min, max);
        }


        #endregion

    }
}