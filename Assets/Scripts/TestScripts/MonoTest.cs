using System;
using System.Reflection.Emit;
using NaughtyAttributes;
using PragmaInject.Core;
using UnityEngine;

namespace TestScripts
{
    public class MonoTest : MonoBehaviour
    {
        public MonoPrefab _monoPrefab;

        private Container _container;

        [Inject]
        public void Construct(Container container)
        {
            _container = container;
        }

        [Button()]
        private void Spawn()
        {
            _container.InstantiatePrefab(_monoPrefab, null);
        }
    }
}