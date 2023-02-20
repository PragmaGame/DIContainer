using PragmaInject.Core;
using UnityEngine;

namespace TestScripts
{
    public class Test8 : MonoBehaviour
    {
        public TestNonMono _testNonMono;
        
        [Inject]
        private void Construct(TestNonMono nonMono)
        {
            _testNonMono = nonMono;
        }
    }
}