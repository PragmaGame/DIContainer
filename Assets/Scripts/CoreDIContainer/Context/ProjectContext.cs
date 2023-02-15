using UnityEngine;

namespace CoreDIContainer
{
    public sealed class ProjectContext : Context
    {
        private static ProjectContext _instance;
        
        public override Container Container { get; }

        public static ProjectContext Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            Debug.Log("Awake Project Conteext");
        }

        public override void InstallBindings(Container container)
        {
            base.InstallBindings(container);
            Debug.Log($"{GetType().Name} Bindings Installed");
        }
    }
}