using System;
using UnityEngine;

namespace TestScripts
{
    [Serializable]
    public class Test5Generic<T> where T : class, new()
    {
        [SerializeField] public T value;
 
        public Test5Generic()
        {
            value = new T();
        }
    }
}