using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CloudDataService.Helper
{
    public class ClassToListObjectItem
    {
        public static List<object> ToList(object obj)
        {
            List<object> returnarray = new List<object>();

            var props = obj.GetType().GetRuntimeProperties();

            foreach (var prop in props)
            {
                if (validType(prop.PropertyType))
                    returnarray.Add(prop.GetValue(obj));
            }

            return returnarray;
        }

        private static bool validType(Type type)
        {
            if (type == typeof(bool))
                return true;
            if (type == typeof(int))
                return true;
            if (type == typeof(double))
                return true;
            if (type == typeof(double?))
                return true;
            if (type == typeof(DateTime))
                return true;
            if (type == typeof(float))
                return true;
            if (type == typeof(string))
                return true;
            if (type == typeof(byte[]))
                return true;
            if (type == typeof(decimal))
                return true;
            if (type == typeof(bool?))
                return true;

            return false;
        }
    }
}