using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public static class ValidateFields
    {
        public static (bool IsValid, List<string> InvalidFields) Validate<T>(List<string> fields) where T : class
        {
            var entityType = typeof(T);
            var propertyNames = entityType.GetProperties()
                .Select(p => p.Name)
                .ToList();

            var invalidFields = fields
                .Where(f => !propertyNames.Contains(f, StringComparer.OrdinalIgnoreCase))
                .ToList();

            return (invalidFields.Count == 0, invalidFields);
        }
    }
}