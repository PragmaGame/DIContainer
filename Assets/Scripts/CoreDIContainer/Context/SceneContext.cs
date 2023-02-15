using System;
using UnityEngine;

namespace CoreDIContainer
{
    [DefaultExecutionOrder(-10000)]
    public class SceneContext : Context
    {
        public override Container Container { get; }
        
        private void Awake()
        {
            UnityGlobalEvents.OnSceneLoaded?.Invoke(gameObject.scene);
        }

        private void OnDestroy()
        {
            UnityGlobalEvents.OnSceneUnLoaded?.Invoke(gameObject.scene);
        }

        public override void InstallBindings(Container container)
        {
            base.InstallBindings(container);
            Debug.Log($"{GetType().Name} Bindings Installed");
        }
    }
}