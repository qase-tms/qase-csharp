using System;

namespace Qase.Csharp.Commons.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class IgnoreAttribute : Attribute, IQaseAttribute
    {
    }
}
