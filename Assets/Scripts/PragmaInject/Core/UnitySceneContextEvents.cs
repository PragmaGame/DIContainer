using System;

namespace PragmaInject.Core
{
    public static class UnitySceneContextEvents
    {
        public static Action<SceneContext> SceneContextLoadedEvent;
        public static Action<SceneContext> SceneContextUnLoadedEvent;
    }
}