using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YorkSDK.Util
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    Debug.Log(typeof(T).ToString() + " is null");

                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this as T;
            Init();
        }

        public virtual void Init()
        {

        }
    }
}

