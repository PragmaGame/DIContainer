using PragmaInject.Core;
using UnityEngine;

namespace TestScripts
{
    public class TestInstaller : MonoInstaller
    {
        [SerializeField] private Test1 _test1;
        [SerializeField] private Test4[] _test4;
        
        public override void InstallBindings(Container container)
        {
            container.Bind(_test1);
            container.BindMany(_test4);
        }
    }
}