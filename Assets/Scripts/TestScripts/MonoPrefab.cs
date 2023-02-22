using PragmaInject.Core;
using UnityEngine;

namespace TestScripts
{
    public class MonoPrefab : MonoBehaviour
    {
        private Container _container;
        
        private void Awake()
        {
            Debug.Log("Awake");
        }
        
        [Inject]
        public void Construct(Container container)
        {
            Debug.Log("Construct");
            _container = container;
        }
        
        private void Start()
        {
            Debug.Log("Start");
            
        }
        
    }
}