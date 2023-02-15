using System;
using UnityEngine.SceneManagement;

namespace CoreDIContainer
{
    public static class UnityGlobalEvents
    {
        public static Action<Scene> OnSceneLoaded;
        public static Action<Scene> OnSceneUnLoaded;
    }
}