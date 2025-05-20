using System;

namespace Qase.Csharp.Commons.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TitleAttribute : Attribute, IQaseAttribute
    {
        public string Title { get; }

        public TitleAttribute(string title)
        {
            Title = title;
        }
    }
}
