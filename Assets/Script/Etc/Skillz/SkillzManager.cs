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
        /// ���� UI ����
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
        /// ���ھ� ���
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
        /// �й� ó��
        /// </summary>
        public void AbortMatch()
        {
            SkillzCrossPlatform.AbortMatch();
        }

        /// <summary>
        /// ���� ������ ���� ���� �ð�
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
        /// ����ڰ� Skillz ���з� ���ư� �� �ִ��� ���θ� ��Ÿ���� �ο� ���� ��ȯ�մϴ�.
        /// �׷� ���� �����ϸ� ��ų�� ���з� ���ư��ϴ�.
        /// ��� �߿� ȣ��Ǹ� ������ �����ؾ� �մϴ�.
        /// </summary>
        /// <returns>���� ����</returns>
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
        /// �� ������ ���� �ð��� �� ��� ��ȯ�մϴ�(������Ÿ��, �ŷ��� �� ����).
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