using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GB
{
    public class ObjectPooling : MonoBehaviour
    {
        public static ObjectPooling I
        {
            get
            {
                if (_i == null)
                {
                    _i = GameObject.FindObjectOfType<GB.ObjectPooling>();

                    if (_i == null)
                    {
                        GameObject obj = new GameObject("ObjectPooling");
                        _i = obj.AddComponent<ObjectPooling>();
                    }
                }

                return _i;

            }
        }

        private static ObjectPooling _i = null;
        private Dictionary<string, Stack<GameObject>> _pocket = new Dictionary<string, Stack<GameObject>>();
        private Dictionary<string, GameObject> _models = new Dictionary<string, GameObject>();
        /// <summary>
        ///  풀링 할수 있는 갯수 체크 
        /// </summary>
        /// <param name="key">풀링할 오브젝트 이름</param>
        /// <returns>갯수</returns>
        public int GetRemainingUses(string key)
        {
            if (_pocket.ContainsKey(key))
                return _pocket[key].Count;

            return -1;
        }


        /// <summary>
        /// 주머니에서 가져오기
        /// </summary>
        /// <param name="key">가져 올 키</param>
        /// <returns>주머니 오브젝트</returns>
        public GameObject Import(string key)
        {
            if (_pocket.ContainsKey(key))
            {
                if (_pocket[key].Count > 0)
                {
                    GameObject obj = _pocket[key].Pop();
                    obj.transform.SetParent(null);
                    obj.SetActive(true);
                    return obj;
                }
            }

            return null;
        }

        public bool RegistModel(string key, GameObject obj)
        {
            if (obj == null) return false;
            if (_models.ContainsKey(key)) return false;

            _models.Add(key, obj);
            return true;
        }

        public bool CheckModel(string key)
        {

            return _models.ContainsKey(key);
        }


        public GameObject GetModel(string key)
        {
            return _models[key];
        }

        public void Registration(string key, GameObject obj, bool isFiled)
        {
            if (obj == null) return;

            if (obj.GetComponent<PoolingType>() == null)
            {
                PoolingType type = obj.AddComponent<PoolingType>();
                type.Name = key;
            }

            if (!_pocket.ContainsKey(key))
            {
                Stack<GameObject> stack = new Stack<GameObject>();

                if (!isFiled)
                    stack.Push(obj);

                _pocket.Add(key, stack);
                
            }
        }

        /// <summary>
        ///  가상 삭제 - 주머니로 돌려보내기
        /// </summary>
        /// <param name="key">들어갈 주머니 키</param>
        /// <param name="obj">들어갈 오브젝트 </param>
        public void Destroy(GameObject obj)
        {

            if (obj == null) return;
            if (obj.GetComponent<PoolingType>() == null)
            {
                //등록되지않은 오브젝트
                Debug.LogWarning("ObjectPooling - Not Registration Object");
                return;
            }

            PoolingType type = obj.GetComponent<PoolingType>();
            string key = type.Name;

            if (_pocket.ContainsKey(key))
            {
                _pocket[key].Push(obj);
            }
            else
            {
                //등록 되지 않은 오브젝트
                Debug.LogWarning("ObjectPooling - Not Registration Object - " + obj.name);
                return;
            }

            obj.transform.SetParent(transform);
            obj.SetActive(false);

        }














    }
}