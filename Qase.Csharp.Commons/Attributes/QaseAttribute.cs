using System;
using AspectInjector.Broker;
using Qase.Csharp.Commons.Aspects;

namespace Qase.Csharp.Commons.Attributes
{
    /// <summary>
    /// Attribute for enabling Qase feature, like steps, attachments, etc.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [Injection(typeof(QaseAspect))]
    public class QaseAttribute : Attribute
    {
    }
}
