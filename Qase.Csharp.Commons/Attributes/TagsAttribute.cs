using System;
using System.Collections.Generic;
using System.Linq;

namespace Qase.Csharp.Commons.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class TagsAttribute : Attribute, IQaseAttribute
    {
        public List<string> Tags { get; }

        public TagsAttribute(params string[] tags)
        {
            Tags = tags.ToList();
        }
    }
}
