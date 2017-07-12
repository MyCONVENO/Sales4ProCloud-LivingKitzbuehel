using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data.SqlClient;

namespace CloudDataService.Helper
{
    public class SqlBulkInsertHelper
    {

        public static void InsertBigData<T>(List<List<object>> data)
        {
            Type tableType = typeof(T);

            DataTable dt = new DataTable(tableType.Name);
            Dictionary<string, string> tableMap = new Dictionary<string, string>();
            var props = tableType.GetRuntimeProperties();


            foreach (var prop in props)
            {
                if (validType(prop.PropertyType))
                {
                    Type proptype = prop.PropertyType;

                    if (prop.PropertyType == typeof(bool?))
                    {
                        proptype = typeof(bool);
                    }

                    if (prop.PropertyType == typeof(double?))
                    {
                        proptype = typeof(double);
                    }

                    if (prop.PropertyType == typeof(int?))
                    {
                        proptype = typeof(int);
                    }

                    dt.Columns.Add(prop.Name, proptype);
                    tableMap.Add(prop.Name, prop.Name);
                }
            }

            foreach (var par in data)
            {
                DataRow MyDataRow = dt.NewRow();
                MyDataRow.ItemArray = par.ToArray();
                dt.Rows.Add(MyDataRow);
            }

            write(dt, "data source=qvzkzfbxa0.database.windows.net;initial catalog=LIVINGKITZBUEHL;persist security info=True;user id=MyCONVENODBUser;password=California14;MultipleActiveResultSets=True;App=EntityFramework", tableMap);
        }

        private static void write(DataTable datatable, string connString, Dictionary<string, string> tableMap)
        {
            // connect to SQL
            using (var connection = new SqlConnection(connString))
            {
                var bulkCopy = makeSqlBulkCopy(connection, datatable.TableName, tableMap, datatable.Rows.Count);
                bulkCopy.BulkCopyTimeout = 360;

                // set the destination table name
                connection.Open();

                using (var dataTableReader = new DataTableReader(datatable))
                {
                    bulkCopy.WriteToServer(dataTableReader);
                }

                connection.Close();
            }
        }

        private static SqlBulkCopy makeSqlBulkCopy(SqlConnection connection, string tableName, Dictionary<string, string> tableMap, int batchSize)
        {
            var bulkCopy =
                new SqlBulkCopy
                    (
                    connection,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.UseInternalTransaction,
                    null
                    )
                {
                    DestinationTableName = tableName,
                    EnableStreaming = true,
                    BatchSize = batchSize
                };

            tableMap.ToList()
                    .ForEach(kp =>
                    {
                        bulkCopy
                .ColumnMappings
                .Add(kp.Key, kp.Value);
                    });
            return bulkCopy;
        }

        private static bool validType(Type type)
        {
            if (type == typeof(bool))
                return true;
            if (type == typeof(int))
                return true;
            if (type == typeof(double))
                return true;
            if (type == typeof(string))
                return true;
            if (type == typeof(DateTime))
                return true;
            if (type == typeof(byte[]))
                return true;
            if (type == typeof(decimal))
                return true;
            if (type == typeof(double?))
                return true;
            if (type == typeof(bool?))
                return true;
            if (type == typeof(int?))
                return true;

            return false;
        }
    }
}