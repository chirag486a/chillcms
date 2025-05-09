using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Backend.Extensions
{
    public static class ObjectExtensions
    {
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            try
            {
                var results = obj.GetType()
                          .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                          .ToDictionary(p => p.Name, p => p.GetValue(obj));
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}