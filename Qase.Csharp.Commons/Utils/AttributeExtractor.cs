#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons.Utils
{
    /// <summary>
    /// Extracts Qase attributes from pre-unwrapped attribute collections
    /// and applies them to a TestResult object.
    /// </summary>
    /// <remarks>
    /// Processes class-level attributes first, then method-level attributes.
    /// Method-level attributes overwrite class-level values for assignment-based
    /// properties (TestopsIds, Title, Suites). Fields use indexer semantics
    /// so duplicate keys do not throw -- method-level values overwrite class-level.
    /// </remarks>
    public static class AttributeExtractor
    {
        /// <summary>
        /// Applies Qase attributes to the given test result.
        /// Class attributes are processed first, then method attributes,
        /// so method-level values take precedence.
        /// </summary>
        /// <param name="classAttributes">Class-level Qase attributes (may be null).</param>
        /// <param name="methodAttributes">Method-level Qase attributes (may be null).</param>
        /// <param name="testResult">The test result to populate.</param>
        public static void Apply(
            IEnumerable<Attribute>? classAttributes,
            IEnumerable<Attribute>? methodAttributes,
            TestResult testResult)
        {
            foreach (var attr in (classAttributes ?? Enumerable.Empty<Attribute>())
                .Concat(methodAttributes ?? Enumerable.Empty<Attribute>()))
            {
                switch (attr)
                {
                    case QaseIdsAttribute qaseIds:
                        testResult.TestopsIds = qaseIds.Ids;
                        break;
                    case TitleAttribute title:
                        testResult.Title = title.Title;
                        break;
                    case FieldsAttribute fields:
                        testResult.Fields[fields.Key] = fields.Value;
                        break;
                    case SuitesAttribute suites:
                        testResult.Relations.Suite.Data = suites.Suites
                            .Select(s => new SuiteData { Title = s })
                            .ToList();
                        break;
                    case IgnoreAttribute:
                        testResult.Ignore = true;
                        break;
                }
            }
        }
    }
}
