using UnityEngine;

namespace PragmaInject.Core
{
    public sealed class ProjectContext : Context
    {
        private static ProjectContext _instance;

        public static ProjectContext Instance => _instance;

        private void Awake()
        {
            Validate();
        }

        private void Validate()
        {
            if (_instance == null)
            {
                _instance = this;
                
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                throw new UnityException("More than one ProjectContext");
                //Destroy(gameObject);
            }
        }
    }
}