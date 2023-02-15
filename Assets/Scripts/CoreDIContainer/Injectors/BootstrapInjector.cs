using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.SceneManagement;

[assembly: AlwaysLinkAssembly]
namespace CoreDIContainer.Injectors
{
    internal static class BootstrapInjector
    {
        public static Container projectContainer;

        public const string PATH_PROJECT_CONTEXT = "ProjectContext";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ProjectInitialization()
        {
            CreateProjectContext();
            
            // UnityGlobalEvents.OnSceneLoaded += SceneLoadedHandler;
            // UnityGlobalEvents.OnSceneUnLoaded += SceneUnLoadedHandler;

            SceneManager.sceneLoaded += SceneLoadedHandler;
            SceneManager.sceneUnloaded += SceneUnLoadedHandler;
        }
        
        private static void CreateProjectContext()
        {
            projectContainer = new Container();
            
            Application.quitting += () =>
            {
                projectContainer.Dispose();
                projectContainer = null;
            };
            
            Debug.Log("Pre Load");
            
            var prefabContext = Resources.Load<ProjectContext>(PATH_PROJECT_CONTEXT);

            Object.Instantiate(prefabContext);
            
            Debug.Log("Post Load");
        }

        private static void SceneLoadedHandler(Scene scene, LoadSceneMode sceneMode)
        {
            Debug.Log("Scene Loaded");
            var container = projectContainer.CreateSubContainer();
        }

        private static void SceneUnLoadedHandler(Scene scene)
        {
            Debug.Log("Scene UnLoaded");
        }
        
        private static IEnumerable<MonoBehaviour> GetAllMonoBehaviourFromScene(Scene scene)
        {
            return scene
                .GetRootGameObjects()
                .SelectMany(gameObject => gameObject.GetComponentsInChildren<MonoBehaviour>(true))
                .Where(m => m != null);
        }
    }
}