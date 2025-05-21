using System;
using AspectInjector.Broker;
using Qase.Csharp.Commons.Aspects;

namespace Qase.Csharp.Commons.Attributes
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [Injection(typeof(StepAspect))]
    public class StepAttribute : Attribute
    {
    }
}
