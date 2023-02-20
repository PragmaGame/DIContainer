using System.Collections.Generic;
using UnityEngine;

namespace PragmaInject.Core
{
    public abstract class Context : MonoBehaviour
    {
        [SerializeField] private List<MonoInstaller> _installers = new();
        
        public Container Container { get; private set; }
        
        public virtual void InstallBindings(Container container)
        {
            Container = container;
            
            foreach (var installer in _installers)
            {
                installer.InstallBindings(container);
            }
            
            container.InjectDependenciesInBinders();
        }
    }
}