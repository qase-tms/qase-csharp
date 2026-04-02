using System;
using System.Collections.Generic;
using System.Linq;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Reqnroll.Reporter
{
    /// <summary>
    /// Parses Reqnroll/Gherkin tags and applies Qase metadata to TestResult.
    /// Supported tags: @QaseId:123, @QaseTitle:My_Title, @QaseFields:key:value,
    /// @QaseSuite:Parent\Child, @QaseIgnore
    /// </summary>
    public static class ReqnrollTagParser
    {
        private const string QaseIdPrefix = "qaseid:";
        private const string QaseTitlePrefix = "qasetitle:";
        private const string QaseFieldsPrefix = "qasefields:";
        private const string QaseSuitePrefix = "qasesuite:";
        private const string QaseIgnoreTag = "qaseignore";

        /// <summary>
        /// Applies Qase metadata from Gherkin tags to a TestResult.
        /// </summary>
        public static void ApplyTags(TestResult result, string[]? tags)
        {
            if (tags == null || tags.Length == 0)
                return;

            foreach (var tag in tags)
            {
                var lower = tag.ToLowerInvariant();

                if (lower.StartsWith(QaseIdPrefix))
                {
                    ApplyQaseIds(result, tag.Substring(QaseIdPrefix.Length));
                }
                else if (lower.StartsWith(QaseTitlePrefix))
                {
                    result.Title = tag.Substring(QaseTitlePrefix.Length).Replace("_", " ");
                }
                else if (lower.StartsWith(QaseFieldsPrefix))
                {
                    ApplyQaseField(result, tag.Substring(QaseFieldsPrefix.Length));
                }
                else if (lower.StartsWith(QaseSuitePrefix))
                {
                    ApplyQaseSuite(result, tag.Substring(QaseSuitePrefix.Length));
                }
                else if (lower == QaseIgnoreTag)
                {
                    result.Ignore = true;
                }
            }
        }

        /// <summary>
        /// Returns true if any tag contains a QaseId.
        /// </summary>
        public static bool HasQaseId(string[]? tags)
        {
            if (tags == null) return false;
            return tags.Any(t => t.ToLowerInvariant().StartsWith(QaseIdPrefix));
        }

        private static void ApplyQaseIds(TestResult result, string value)
        {
            var parts = value.Split(',');
            var ids = new List<long>();

            foreach (var part in parts)
            {
                if (long.TryParse(part.Trim(), out var id))
                {
                    ids.Add(id);
                }
            }

            if (ids.Count > 0)
            {
                result.TestopsIds = ids;
            }
        }

        private static void ApplyQaseField(TestResult result, string value)
        {
            var colonIndex = value.IndexOf(':');
            if (colonIndex > 0 && colonIndex < value.Length - 1)
            {
                var key = value.Substring(0, colonIndex);
                var val = value.Substring(colonIndex + 1);
                result.Fields[key] = val;
            }
        }

        private static void ApplyQaseSuite(TestResult result, string value)
        {
            var parts = value.Split('\\');
            result.Relations ??= new Relations();
            result.Relations.Suite.Data = parts
                .Select(p => new SuiteData { Title = p })
                .ToList();
        }
    }
}
