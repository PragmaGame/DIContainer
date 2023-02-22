using UnityEngine;
using UnityEngine.Scripting;

[assembly: AlwaysLinkAssembly]
namespace PragmaInject.Core
{
    internal static class RootInject
    {
        private static Container _projectContainer;

        private const string PATH_PROJECT_CONTEXT = "ProjectContext";
        
        public static Transform InvisibleSpawnContainer { get; private set; }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ProjectInitialization()
        {
            CreateSpawnContainer();
            CreateProjectContext();

            Application.quitting += DestroyProjectContext;

            UnitySceneContextEvents.SceneContextLoadedEvent += SceneLoadedHandler;
            UnitySceneContextEvents.SceneContextUnLoadedEvent += SceneUnLoadedHandler;
        }
        
        private static void CreateProjectContext()
        {
            _projectContainer = new Container();

            var prefabContext = Resources.Load<ProjectContext>(PATH_PROJECT_CONTEXT);

            var context = Object.Instantiate(prefabContext);
            
            context.InstallBindings(_projectContainer);
        }

        private static void SceneLoadedHandler(SceneContext context)
        {
            var container = _projectContainer.CreateSubContainer();
            
            context.InstallBindings(container);
            
            CreateSpawnContainer();
        }

        private static void SceneUnLoadedHandler(SceneContext context)
        {
            context.Container.Dispose();
            
            InvisibleSpawnContainer = null;
        }

        private static void DestroyProjectContext()
        {
            _projectContainer.Dispose();
            _projectContainer = null;
        }

        private static void CreateSpawnContainer()
        {
            var spawnContainer = new GameObject(nameof(InvisibleSpawnContainer))
            {
                hideFlags = HideFlags.HideInHierarchy
            };

            spawnContainer.SetActive(false);

            InvisibleSpawnContainer = spawnContainer.transform;
        }
    }
}