using JetBrains.Annotations;
using UnityEngine;

namespace Common
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T item;

        public static T Instance
        {
            get
            {
                if (item != null)
                {
                    return item;
                }

                item = FindObjectOfType<T>();

                if (item == null)
                {
                    item = new GameObject(typeof(T).ToString()).AddComponent<T>();
                }

                return item;
            }
        }

        [UsedImplicitly]
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }

            if (Instance != null && Instance.transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}