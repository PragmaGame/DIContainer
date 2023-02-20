using UnityEngine;
using UnityEngine.Scripting;

[assembly: AlwaysLinkAssembly]
namespace PragmaInject.Core
{
    internal static class RootInject
    {
        private static Container _projectContainer;

        private const string PATH_PROJECT_CONTEXT = "ProjectContext";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ProjectInitialization()
        {
            CreateProjectContext();
            
            UnitySceneContextEvents.SceneContextLoadedEvent += SceneLoadedHandler;
            UnitySceneContextEvents.SceneContextUnLoadedEvent += SceneUnLoadedHandler;
        }
        
        private static void CreateProjectContext()
        {
            _projectContainer = new Container();
            
            Application.quitting += () =>
            {
                _projectContainer.Dispose();
                _projectContainer = null;
            };
            
            var prefabContext = Resources.Load<ProjectContext>(PATH_PROJECT_CONTEXT);

            var context = Object.Instantiate(prefabContext);
            
            context.InstallBindings(_projectContainer);
        }

        private static void SceneLoadedHandler(SceneContext context)
        {
            var container = _projectContainer.CreateSubContainer();
            
            context.InstallBindings(container);
        }

        private static void SceneUnLoadedHandler(SceneContext context)
        {
            context.Container.Dispose();
        }
    }
}