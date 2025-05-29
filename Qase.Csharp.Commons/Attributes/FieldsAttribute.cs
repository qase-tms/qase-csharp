using System;

namespace Qase.Csharp.Commons.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class FieldsAttribute : Attribute, IQaseAttribute
    {
        public string Key { get; }
        public string Value { get; }

        public FieldsAttribute(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
