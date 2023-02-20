using UnityEngine;

namespace PragmaInject.Core
{
    public class AutoBinding : MonoBehaviour
    {
        [SerializeField] private Component[] _components;

        public Component[] Bindings => _components;
    }
}