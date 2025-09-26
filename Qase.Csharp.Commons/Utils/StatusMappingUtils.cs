using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons.Utils
{
    /// <summary>
    /// Utility class for handling test result status mapping
    /// </summary>
    public static class StatusMappingUtils
    {
        /// <summary>
        /// Valid status values for mapping
        /// </summary>
        public static readonly HashSet<string> ValidStatuses = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "passed",
            "failed", 
            "skipped",
            "blocked",
            "invalid"
        };

        /// <summary>
        /// Parses status mapping from environment variable format
        /// </summary>
        /// <param name="statusMappingString">Status mapping string in format "invalid=failed,skipped=passed"</param>
        /// <param name="logger">Logger for warnings</param>
        /// <returns>Dictionary of status mappings</returns>
        public static Dictionary<string, string> ParseStatusMappingFromEnv(string? statusMappingString, ILogger? logger = null)
        {
            var mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(statusMappingString))
            {
                return mapping;
            }

            try
            {
                var pairs = statusMappingString!.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var pair in pairs)
                {
                    var trimmedPair = pair.Trim();
                    if (string.IsNullOrEmpty(trimmedPair)) continue;
                    
                    var parts = trimmedPair.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        var fromStatus = parts[0].Trim().ToLowerInvariant();
                        var toStatus = parts[1].Trim().ToLowerInvariant();
                        
                        if (ValidateStatusMapping(fromStatus, toStatus, logger))
                        {
                            mapping[fromStatus] = toStatus;
                        }
                    }
                    else
                    {
                        logger?.LogWarning("Invalid status mapping format: '{Pair}'. Expected format: 'from=to'", trimmedPair);
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Failed to parse status mapping from string: '{StatusMappingString}'", statusMappingString);
            }

            return mapping;
        }

        /// <summary>
        /// Parses status mapping from JSON element
        /// </summary>
        /// <param name="statusMappingElement">Status mapping JSON element</param>
        /// <param name="logger">Logger for warnings</param>
        /// <returns>Dictionary of status mappings</returns>
        public static Dictionary<string, string> ParseStatusMappingFromJson(System.Text.Json.JsonElement statusMappingElement, ILogger? logger = null)
        {
            var mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                foreach (var property in statusMappingElement.EnumerateObject())
                {
                    var fromStatus = property.Name.Trim().ToLowerInvariant();
                    var toStatus = property.Value.GetString()?.Trim().ToLowerInvariant();
                    
                    if (!string.IsNullOrEmpty(toStatus) && ValidateStatusMapping(fromStatus, toStatus!, logger))
                    {
                        mapping[fromStatus] = toStatus;
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Failed to parse status mapping from JSON element");
            }

            return mapping;
        }

        /// <summary>
        /// Validates a status mapping pair
        /// </summary>
        /// <param name="fromStatus">Source status</param>
        /// <param name="toStatus">Target status</param>
        /// <param name="logger">Logger for warnings</param>
        /// <returns>True if mapping is valid</returns>
        public static bool ValidateStatusMapping(string fromStatus, string toStatus, ILogger? logger = null)
        {
            if (!ValidStatuses.Contains(fromStatus))
            {
                logger?.LogWarning("Invalid source status '{FromStatus}' in status mapping. Valid statuses: {ValidStatuses}", 
                    fromStatus, string.Join(", ", ValidStatuses));
                return false;
            }

            if (!ValidStatuses.Contains(toStatus))
            {
                logger?.LogWarning("Invalid target status '{ToStatus}' in status mapping. Valid statuses: {ValidStatuses}", 
                    toStatus, string.Join(", ", ValidStatuses));
                return false;
            }

            if (string.Equals(fromStatus, toStatus, StringComparison.OrdinalIgnoreCase))
            {
                logger?.LogWarning("Status mapping from '{FromStatus}' to '{ToStatus}' is redundant (same status)", 
                    fromStatus, toStatus);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Applies status mapping to a test result
        /// </summary>
        /// <param name="testResult">Test result to apply mapping to</param>
        /// <param name="statusMapping">Status mapping dictionary</param>
        /// <param name="logger">Logger for status change notifications</param>
        /// <returns>True if status was changed</returns>
        public static bool ApplyStatusMapping(TestResult testResult, Dictionary<string, string> statusMapping, ILogger? logger = null)
        {
            if (testResult?.Execution == null || statusMapping == null || statusMapping.Count == 0)
            {
                return false;
            }

            var currentStatus = testResult.Execution.Status.ToString().ToLowerInvariant();
            
            if (statusMapping.TryGetValue(currentStatus, out var mappedStatus))
            {
                if (Enum.TryParse<TestResultStatus>(mappedStatus, true, out var newStatus))
                {
                    var oldStatus = testResult.Execution.Status;
                    testResult.Execution.Status = newStatus;
                    
                    logger?.LogInformation("Status mapping applied: '{OldStatus}' -> '{NewStatus}' for test '{TestId}'", 
                        oldStatus, newStatus, testResult.Id);
                    
                    return true;
                }
                else
                {
                    logger?.LogWarning("Failed to parse mapped status '{MappedStatus}' for test '{TestId}'", 
                        mappedStatus, testResult.Id);
                }
            }

            return false;
        }

        /// <summary>
        /// Converts string status to TestResultStatus enum
        /// </summary>
        /// <param name="statusString">Status string</param>
        /// <returns>TestResultStatus enum value or null if invalid</returns>
        public static TestResultStatus? ParseStatus(string? statusString)
        {
            if (string.IsNullOrWhiteSpace(statusString))
            {
                return null;
            }

            return Enum.TryParse<TestResultStatus>(statusString, true, out var status) ? status : null;
        }

        /// <summary>
        /// Gets all valid status names
        /// </summary>
        /// <returns>Array of valid status names</returns>
        public static string[] GetValidStatusNames()
        {
            return ValidStatuses.ToArray();
        }
    }
}
