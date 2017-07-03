using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CloudDataService.Helper
{
    class DatabaseModelHelper
    {
        public static bool Update(object sourceObj, object destinationObj)
        {
            bool updateMe = false;
            var dbtypeName = sourceObj.GetType().Name;
            string idname = dbtypeName + "ID";
            foreach (var sourceprop in sourceObj.GetType().GetProperties())
            {
                if (validType(sourceprop))
                {
                    if (sourceprop.Name != "SyncDateTime" && sourceprop.Name != idname)
                    {
                        if (sourceprop.CanWrite)
                        {
                            var destprop = destinationObj.GetType().GetProperty(sourceprop.Name);
                            if (destprop != null && destprop.CanWrite)
                            {
                                var sourceValue = sourceprop.GetValue(sourceObj, null);
                                var destinationValue = destprop.GetValue(destinationObj, null);
                                if (destinationValue == null && sourceValue != null || sourceValue != null && sourceValue.ToString() != destinationValue.ToString())
                                {
                                    if (sourceValue.GetType() == typeof(decimal))
                                    {
                                        if ((decimal)sourceValue != (decimal)destinationValue)
                                        {
                                            destprop.SetValue(destinationObj, sourceValue);
                                            updateMe = true;
                                        }
                                    }
                                    else
                                    {
                                        destprop.SetValue(destinationObj, sourceValue);
                                        updateMe = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return updateMe;
        }

        public static bool ImportData<T>(LIVINGKITZBUEHLEntities dataModel, List<T> newItems, List<T> existingItems)
        {
            if (!newItems.Any())
                return false;

            var firstItem = newItems.First();

            List<List<object>> insertItems = new List<List<object>>();

            List<T> updateItems = new List<T>();
            List<T> deleteItems = new List<T>();
            string databaseTableName = typeof(T).Name;

            var primaryKeyPropName = databaseTableName + "ID";
            var primaryKeyProp = firstItem.GetType().GetRuntimeProperty(primaryKeyPropName);
            var syncDateTimeProp = firstItem.GetType().GetRuntimeProperty("SyncDateTime");
            var isDeletedProp = firstItem.GetType().GetRuntimeProperty("IsDeleted");

            //var existingItems = dataModel.Color.Where(m => m.IsDeleted == false).ToList();
            deleteItems.AddRange(existingItems);

            foreach (var m in newItems)
            {

                if (string.IsNullOrEmpty(primaryKeyProp.GetValue(m).ToString()))
                {
                    primaryKeyProp.SetValue(m, Guid.NewGuid().ToString());
                    insertItems.Add(ClassToListObjectItem.ToList(m));
                }
                else
                {
                    var existingDBItem = existingItems.FirstOrDefault(dbitem => primaryKeyProp.GetValue(dbitem).ToString() == primaryKeyProp.GetValue(m).ToString());
                    if (existingDBItem != null && DatabaseModelHelper.Update(m, existingDBItem))
                    {
                        syncDateTimeProp.SetValue(existingDBItem, DateTime.Now);
                        isDeletedProp.SetValue(existingDBItem, false);
                        updateItems.Add(m);
                    }
                    deleteItems.Remove(existingDBItem);
                }

            }



            foreach (var dbitem in deleteItems)
            {
                syncDateTimeProp.SetValue(dbitem, DateTime.Now);
                isDeletedProp.SetValue(dbitem, true);
            }

            dataModel.SaveChanges();

            SqlBulkInsertHelper.InsertBigData<T>(insertItems);

            return true;
        }

        static bool validType(PropertyInfo prop)
        {
            if (prop.PropertyType == typeof(string))
                return true;
            if (prop.PropertyType == typeof(int))
                return true;
            if (prop.PropertyType == typeof(double))
                return true;
            if (prop.PropertyType == typeof(float))
                return true;
            if (prop.PropertyType == typeof(decimal))
                return true;
            if (prop.PropertyType == typeof(byte))
                return true;
            if (prop.PropertyType == typeof(DateTime))
                return true;
            if (prop.PropertyType == typeof(bool))
                return true;
            if (prop.PropertyType == typeof(bool?))
                return true;

            return false;

        }
    }
}