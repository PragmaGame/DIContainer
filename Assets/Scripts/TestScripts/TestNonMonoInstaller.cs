using PragmaInject.Core;

namespace TestScripts
{
    public class TestNonMonoInstaller : MonoInstaller
    {
        public override void InstallBindings(Container container)
        {
            container.Bind(new TestNonMono());
        }
    }
}