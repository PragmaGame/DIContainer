using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PragmaInject.Core
{
    public class Container : IDisposable
    {
        private const BindingFlags FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        
        private Container _rootContainer;
        
        private readonly List<Container> _subContainers;
        private readonly List<object> _bindings;

        public IReadOnlyList<Container> SubContainers => _subContainers;

        public Container(Container root = null)
        {
            _rootContainer = root;
            
            _subContainers = new List<Container>();
            _bindings = new List<object> { this };
        }

        public Container CreateSubContainer()
        {
            var subContainer = new Container(this);
            
            _subContainers.Add(subContainer);

            return subContainer;
        }

        private void RemoveSubContainer(Container container)
        {
            _subContainers.Remove(container);
        }

        public void DestroySubContainer(Container container)
        {
            _subContainers.Find(x => x == container)?.Dispose();
        }

        public void Dispose()
        {
            for (var i = _subContainers.Count - 1; i >= 0; i--)
            {
                _subContainers[i].Dispose();
            }

            if (_rootContainer == null)
            {
                return;
            }
            
            _rootContainer.RemoveSubContainer(this);
            _rootContainer = null;

            _subContainers.Clear();
            _bindings.Clear();
        }

        public void Bind<T>(T item) where T : class
        {
            _bindings.Add(item);
        }
        
        public void BindMany<T>(IEnumerable<T> items) where T : class
        {
            _bindings.AddRange(items);
        }

        public T Resolve<T>(bool isRecursiveSearch = true) where T : class
        {
            if((TryGetSingleDependencyByType(typeof(T), isRecursiveSearch, out var dependency)))
            {
                return dependency as T;
            }

            return null;
        }

        public IList<T> ManyResolve<T>(bool isRecursiveSearch = true) where T : class
        {
            var dependency = GetManyDependencyByType(typeof(T), isRecursiveSearch, out var isFoundAtLeastOne);

            return dependency as IList<T>;
        }

        public void InjectInBinders()
        {
            foreach (var item in _bindings)
            {
                InjectInObject(item);
            }
        }
        
        public void InjectInObjects(IEnumerable<object> objects, bool isNeedReinject = false)
        {
            foreach (var obj in objects)
            {
                if (_bindings.Contains(obj) && !isNeedReinject)
                {
                    continue;
                }
                
                InjectInObject(obj);
            }
        }

        public void InjectInMono(Component monoComponent, bool isRecursive)
        {
            var objects = isRecursive ? monoComponent.GetComponentsInChildren<Component>()
                : monoComponent.GetComponents<Component>();

            foreach (var obj in objects)
            {
                InjectInObject(obj);
            }
        }

        public void InjectInObject(object target)
        {
            var methods = new List<MethodInfo>();
            
            RecursiveDataCollection(methods, target.GetType());
            
            InjectAllMethods(target, methods);
        }

        private void RecursiveDataCollection(List<MethodInfo> methodInfos, Type type)
        {
            if (type == null)
            {
                return;
            }
            
            RecursiveDataCollection(methodInfos, type.BaseType);
            
            var methods = type.GetMethods(FLAGS).Where(method => method.IsDefined(typeof(Inject), false)).ToArray();
            
            methodInfos.AddRange(methods);
        }

        private void InjectAllMethods(object target, IEnumerable<MethodInfo> methodsInfo)
        {
            foreach (var methodInfo in methodsInfo)
            {
                InjectMethod(target, methodInfo);
            }
        }

        private void InjectMethod(object target, MethodBase methodInfo)
        {
            var parametersInfo = methodInfo.GetParameters();
            var parameters = new object[parametersInfo.Length];

            var injectAttribute = methodInfo.GetCustomAttribute(typeof(Inject), false) as Inject;

            var index = 0;
            
            foreach (var parameterInfo in parametersInfo)
            {
                if (TryGetDependencyByType(parameterInfo.ParameterType, injectAttribute.IsRecursiveSearch , out var foundDependency))
                {
                    parameters[index] = foundDependency;
                    index++;
                }
                else
                {
                    return;
                }
            }

            methodInfo.Invoke(target, parameters);
        }

        private bool TryGetDependencyByType(Type type, bool isRecursiveSearch, out object dependency)
        {
            if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                dependency = GetManyDependencyByType(type, isRecursiveSearch, out var isFoundAtLeastOne);

                return isFoundAtLeastOne;
            }
            
            return TryGetSingleDependencyByType(type, isRecursiveSearch, out dependency);;
        }

        private IList GetManyDependencyByType(Type type, bool isRecursive, out bool isFoundAtLeastOne)
        {
            var elementType = type.GetGenericArguments()[0];
            
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(elementType);
            var collect = (IList)Activator.CreateInstance(constructedListType);

            GetManyDependencyByType(elementType, collect, isRecursive);

            isFoundAtLeastOne = collect.Count > 0;
            
            return collect;
        }

        private void GetManyDependencyByType(Type elementType, IList collect, bool isRecursive)
        {
            foreach (var item in _bindings)
            {
                if (elementType.IsInstanceOfType(item))
                {
                    collect.Add(item);
                }
            }

            if (isRecursive && _rootContainer != null)
            {
                _rootContainer.GetManyDependencyByType(elementType, collect, true);
            }
        }

        private bool TryGetSingleDependencyByType(Type type, bool isRecursive, out object dependency)
        {
            foreach (var item in _bindings)
            {
                if (type.IsInstanceOfType(item))
                {
                    dependency = item;
                    return true;
                }
            }

            if (isRecursive && _rootContainer != null)
            {
                if (_rootContainer.TryGetSingleDependencyByType(type, true, out var dependencyRoot))
                {
                    dependency = dependencyRoot;
                    return true;
                }
            }
            
            dependency = null;
            return false;
        }
        
        public T InstantiatePrefab<T>(T original, Transform container = null) where T : Component
        {
            return InstantiatePrefab(container,(parent) => UnityEngine.Object.Instantiate(original, parent));
        }
        
        public T InstantiatePrefab<T>(T original, Transform container, bool isWorldPositionStays) where T : Component
        {
            return InstantiatePrefab(container, (parent) => UnityEngine.Object.Instantiate(original, parent, isWorldPositionStays));
        }

        public T InstantiatePrefab<T>(T original, Vector3 position, Quaternion rotation, Transform container = null) where T : Component
        {
            return InstantiatePrefab(container, (parent) => UnityEngine.Object.Instantiate(original, position, rotation, parent));
        }

        private T InstantiatePrefab<T>(Transform parent, Func<Transform, T> instantiateMethod)
            where T : Component
        {
            var instance = instantiateMethod.Invoke(RootInject.InvisibleSpawnContainer);

            InjectInMono(instance,true);

            instance.transform.SetParent(parent, false);

            return instance;
        }
    }
}
