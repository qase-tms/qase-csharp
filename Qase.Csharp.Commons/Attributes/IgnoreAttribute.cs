using System;

namespace Qase.Csharp.Commons.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IgnoreAttribute : Attribute, IQaseAttribute
    {
    }
}
