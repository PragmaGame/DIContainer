using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PragmaInject.Core
{
    [DefaultExecutionOrder(-10000)]
    public class SceneContext : Context
    {
        private void Awake()
        {
            Validated();
            
            UnitySceneContextEvents.SceneContextLoadedEvent?.Invoke(this);
        }

        private void OnDestroy()
        {
            UnitySceneContextEvents.SceneContextUnLoadedEvent?.Invoke(this);
        }

        public override void InstallBindings(Container container)
        {
            var behaviours = GetAllObjectFromScene<MonoBehaviour>().ToArray();

            foreach (var mono in behaviours)
            {
                if (mono is AutoBinding convertAutoBinding)
                {
                    container.BindMany(convertAutoBinding.Bindings);
                }
            }

            base.InstallBindings(container);

            container.InjectDependenciesInObjects(behaviours);
        }

        private void Validated()
        {
            var sceneContexts = GetAllObjectFromScene<SceneContext>();

            if (sceneContexts.Count() > 1)
            {
                throw new UnityException("More than one SceneContext");
            }
        }
        
        private IEnumerable<T> GetAllObjectFromScene<T>()
        {
            return gameObject.scene
                .GetRootGameObjects()
                .SelectMany(x => x.GetComponentsInChildren<T>(true))
                .Where(m => m != null);
        }
    }
}