using System;
using JetBrains.Annotations;

namespace CoreDIContainer
{
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    [AttributeUsage(AttributeTargets.Method)]
    public class Inject : Attribute
    {
        public bool IsRecursiveSearch { get; }
        
        public Inject()
        {
            IsRecursiveSearch = true;
        }

        public Inject(bool isRecursiveSearch)
        {
            IsRecursiveSearch = isRecursiveSearch;
        }
    }
}