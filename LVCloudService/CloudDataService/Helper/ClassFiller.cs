using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CloudDataService.Helper
{
    public class ClassFiller
    {
        public static T GetFilledClass<T>(object sourceObj)
        {
            string jsonString = JsonConvert.SerializeObject(sourceObj);
            return JsonConvert.DeserializeObject<T>(jsonString);

            //if (sourceObj == null)
            //{
            //    sourceObj = new object();
            //}

            //if (destinationObj == null)
            //{
            //    destinationObj = new object();
            //}

            //foreach (var prop in destinationObj.GetType().GetRuntimeProperties())
            //{
            //    var sourceProp = sourceObj.GetType().GetRuntimeProperty(prop.Name);

            //    if (prop.CanWrite && sourceProp != null)
            //    {
            //        var sObj = sourceProp.GetValue(sourceObj);
            //        var dObj = prop.GetValue(destinationObj);

            //        FillClass(sObj, dObj);

            //        if (prop.CustomAttributes.Where(ca => ca.AttributeType.Name == "IgnoreFillAttrib").Count() == 0
            //            && sourceProp.CustomAttributes.Where(ca => ca.AttributeType.Name == "IgnoreFillAttrib").Count() == 0
            //            )
            //        {
            //            prop.SetValue(destinationObj, sourceProp.GetValue(sourceObj));
            //        }
            //    }
            //}
        }

        public static void PasteData(object sourceObj, object destObj)
        {
            foreach (var sourceprop in sourceObj.GetType().GetProperties())
            {
                if (sourceprop.CanWrite)
                {
                    var destprop = destObj.GetType().GetProperty(sourceprop.Name);
                    if (destprop != null && destprop.CanWrite)
                    {
                        object sourceValue = sourceprop.GetValue(sourceObj, null);
                        if (sourceValue != null)
                        {
                            if (destObj.GetType() == typeof(DateTime))
                            {
                                long SourceTicks = ((DateTime)sourceValue).Ticks;
                                destprop.SetValue(destObj, new DateTime(SourceTicks, DateTimeKind.Unspecified), null);
                            }
                            else
                            {
                                destprop.SetValue(destObj, sourceValue, null);
                            }
                        }
                    }
                }
            }
        }

        public static void FillDefaultValues(object item)
        {
            foreach (var prop in item.GetType().GetProperties())
            {
                if (prop.CanWrite)
                {


                    if (prop.PropertyType == typeof(string))
                    {
                        prop.SetValue(item, string.Empty, null);
                    }
                    if (prop.PropertyType == typeof(int))
                    {
                        prop.SetValue(item, 0, null);
                    }
                    if (prop.PropertyType == typeof(DateTime))
                    {
                        prop.SetValue(item, new DateTime(1950, 1, 1), null);
                    }
                    if (prop.PropertyType == typeof(long))
                    {
                        prop.SetValue(item, 0, null);
                    }
                    if (prop.PropertyType == typeof(double))
                    {
                        prop.SetValue(item, 0.0, null);
                    }
                    if (prop.PropertyType == typeof(byte[]))
                    {
                        prop.SetValue(item, null, null);
                    }
                }
            }
        }

        public static T GetFilledClassFromString<T>(string source, string[] propertyString)
        {
            T newInstance = Activator.CreateInstance<T>();
            newInstance.GetType().GetRuntimeProperty("ID").SetValue(newInstance, Guid.NewGuid().ToString());

            foreach (string propString in propertyString)
            {
                var prop = newInstance.GetType().GetRuntimeProperty(propString.Split(';')[2]);
                if (prop == null)
                    continue;

                string newValue = string.Empty;

                if (propString.Split(';')[1] != string.Empty)
                {
                    int end = 0;
                    end = Convert.ToInt32(propString.Split(';')[1]);

                    if (source.Length < Convert.ToInt32(propString.Split(';')[0]) + end)
                        end = source.Length - Convert.ToInt32(propString.Split(';')[0]);

                    if (end > 0)
                        newValue = source.Substring(Convert.ToInt32(propString.Split(';')[0]), end);
                }
                else
                {
                    newValue = source.Substring(Convert.ToInt32(propString.Split(';')[0]));
                }
                newValue = newValue.Trim();
                if (newValue.Length > 50)
                    newValue.ToString();

                

                prop.SetValue(newInstance, newValue);
            }
            return newInstance;
        }
    }
}
