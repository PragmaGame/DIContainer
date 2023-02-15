using System.Collections.Generic;
using System.Linq;
using CoreDIContainer;
using UnityEngine;

namespace TestScripts
{
    public class Test1 : MonoBehaviour
    {
        public List<Test4> test4s;

        public Test3 test3;
        
        [Inject]
        private void Construct(Test3 test3, IEnumerable<Test4> test4s)
        {
            this.test3 = test3;
            this.test4s = test4s.ToList();
        }
    }
}