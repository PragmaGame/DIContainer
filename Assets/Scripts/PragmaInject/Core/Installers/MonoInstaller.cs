using UnityEngine;

namespace PragmaInject.Core
{
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        public abstract void InstallBindings(Container container);
    }
}