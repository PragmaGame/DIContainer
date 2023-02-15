using System.Collections.Generic;
using UnityEngine;

namespace CoreDIContainer
{
    public abstract class Context : MonoBehaviour
    {
        [SerializeField] private List<MonoInstaller> _installers = new();
        
        public abstract Container Container { get; }
        
        public virtual void InstallBindings(Container container)
        {
            foreach (var installer in _installers)
            {
                installer.InstallBindings(container);
            }
        }
    }
}