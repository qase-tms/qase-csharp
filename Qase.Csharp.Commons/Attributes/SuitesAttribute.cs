using System;
using System.Collections.Generic;
using System.Linq;
namespace Qase.Csharp.Commons.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SuitesAttribute : Attribute, IQaseAttribute
    {
        public List<string> Suites { get; }

        public SuitesAttribute(params string[] suites)
        {
            Suites = suites.ToList();
        }
    }
}
