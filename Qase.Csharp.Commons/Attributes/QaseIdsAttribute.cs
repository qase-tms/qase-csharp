using System;
using System.Collections.Generic;
using System.Linq;

namespace Qase.Csharp.Commons.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class QaseIdsAttribute : Attribute, IQaseAttribute
    {
        public List<long> Ids { get; }

        public QaseIdsAttribute(params long[] ids)
        {
            Ids = ids.ToList();
        }
    }
}
