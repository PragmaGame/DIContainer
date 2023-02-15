using UnityEngine;

namespace CoreDIContainer
{
    public abstract class MonoInstaller : MonoBehaviour
    {
        public abstract void InstallBindings(Container container);
    }
}