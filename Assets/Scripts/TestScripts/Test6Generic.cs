using System.Collections.Generic;
using CoreDIContainer;
using UnityEngine;

namespace TestScripts
{
    public class Test6Generic : MonoBehaviour
    {
        [SerializeField] private Test5Generic<Test7Generic> test5;
        
        [Inject]
        private void Construct(IEnumerable<Test5Generic<Test7Generic>> test5Generic)
        {
            foreach (var test5GenericA in test5Generic)
            {
                //Debug.Log(test5GenericA.value.value);
            }
        }
    }
}