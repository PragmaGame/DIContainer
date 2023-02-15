using CoreDIContainer;
using UnityEngine;

namespace TestScripts
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] _monoBehaviours;

        private Container _container;

        private void Start()
        {
            _monoBehaviours = GetComponentsInChildren<MonoBehaviour>();
            
            _container = new Container();
            
            var subContainer = _container.CreateSubContainer();

            foreach (var monoBehaviour in _monoBehaviours)
            {
                subContainer.Bind(monoBehaviour);
            }

            for (int i = 0; i < 4; i++)
            {
                var test = new Test5Generic<Test7Generic>();
                test.value = new Test7Generic();
                test.value.value = "Find" + i;
                
                _container.Bind(test);
            }
            
            subContainer.InjectDependencies();
        }
    }
}