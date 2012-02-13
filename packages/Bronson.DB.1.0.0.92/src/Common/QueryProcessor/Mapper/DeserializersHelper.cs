using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Dapper;


namespace Bronson.DB.Common.QueryProcessor.Mapper
{
    public static class DeserializersHelper
    {


        #region helper dictionaries
        private static readonly ConcurrentDictionary<SqlMapper.Identity, object> cachedSerializers = new ConcurrentDictionary<SqlMapper.Identity, object>();
        #endregion


        internal static Func<IDataReader, T> GetDeserializer<T>(SqlMapper.Identity identity, IDataReader reader)
        {
            object oDeserializer;
            if (!DeserializersHelper.cachedSerializers.TryGetValue(identity, out oDeserializer))
            {
                if (typeof(T) == ParametersHelper.DynamicStub.Type || typeof(T) == typeof(ExpandoObject))
                {
                    oDeserializer = GetDynamicDeserializer(reader);
                }
                else if (typeof(T).IsClass && typeof(T) != typeof(string))
                {
                    oDeserializer = GetClassDeserializer<T>(reader);
                }
                else
                {
                    oDeserializer = GetStructDeserializer<T>(reader);
                }

                DeserializersHelper.cachedSerializers[identity] = oDeserializer;
            }
            var deserializer = (Func<IDataReader, T>)oDeserializer;
            return deserializer;
        }

        internal static object GetStructDeserializer<T>(IDataReader reader)
        {
            Func<IDataReader, T> deserializer = null;
            deserializer = r => (T)r.GetValue(0);
            return deserializer;
        }

        internal static Func<IDataReader, T> GetClassDeserializer<T>(IDataReader reader)
        {
            var dm = new DynamicMethod("Deserialize" + Guid.NewGuid(), typeof(T), new Type[] { typeof(IDataReader) }, true);
            var il = dm.GetILGenerator();

            var properties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new { Name = p.Name, Setter = p.GetSetMethod(), Type = p.PropertyType })
                .Where(info => info.Setter != null)
                .ToList();

            var names = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                names.Add(reader.GetName(i));
            }

            var setters = (
                              from n in names
                              select new { Name = n, Info = properties.FirstOrDefault(p => p.Name.ToUpper() == n.ToUpper()) }
                          ).ToList();


            var getItem = typeof(IDataRecord).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetIndexParameters().Any() && p.GetIndexParameters()[0].ParameterType == typeof(int))
                .Select(p => p.GetGetMethod()).First();

            var index = 0;

            // stack is empty
            il.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes)); // stack is now [target]
            foreach (var item in setters)
            {
                if (item.Info != null)
                {
                    il.Emit(OpCodes.Dup); // stack is now [target][target]
                    Label isDbNullLabel = il.DefineLabel();
                    Label finishLabel = il.DefineLabel();

                    il.Emit(OpCodes.Ldarg_0); // stack is now [target][target][reader]
                    DeserializersHelper.EmitInt32(il, index); // stack is now [target][target][reader][index]

                    il.Emit(OpCodes.Callvirt, getItem); // stack is now [target][target][value-as-object]

                    il.Emit(OpCodes.Dup); // stack is now [target][target][value][value]
                    il.Emit(OpCodes.Isinst, typeof(DBNull)); // stack is now [target][target][value-as-object][DBNull or null]
                    il.Emit(OpCodes.Brtrue_S, isDbNullLabel); // stack is now [target][target][value-as-object]

                    il.Emit(OpCodes.Unbox_Any, item.Info.Type); // stack is now [target][target][typed-value]
                    il.Emit(OpCodes.Callvirt, item.Info.Setter); // stack is now [target]
                    il.Emit(OpCodes.Br_S, finishLabel); // stack is now [target]


                    il.MarkLabel(isDbNullLabel); // incoming stack: [target][target][value]
                    il.Emit(OpCodes.Pop); // stack is now [target][target]
                    il.Emit(OpCodes.Pop); // stack is now [target]

                    il.MarkLabel(finishLabel);
                }
                index += 1;
            }
            il.Emit(OpCodes.Ret); // stack is empty

            return (Func<IDataReader, T>)dm.CreateDelegate(typeof(Func<IDataReader, T>));
        }

        internal static object GetDynamicDeserializer(IDataReader reader)
        {
            var colNames = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                colNames.Add(reader.GetName(i));
            }

            Func<IDataReader, ExpandoObject> rval =
                r =>
                    {
                        IDictionary<string, object> row = new ExpandoObject();
                        var i = 0;
                        foreach (var colName in colNames)
                        {
                            var tmp = r.GetValue(i);
                            row[colName] = tmp == DBNull.Value ? null : tmp;
                            i++;
                        }
                        return (ExpandoObject)row;
                    };

            return rval;
        }

        internal static string GetReaderFieldNamesAndTypes<T>(IDataReader reader, Exception ex)
        {
            var retVal = new StringBuilder();

            retVal.AppendLine("-----------------------------------------------------");
            retVal.AppendLine("The following error was thrown : ");
            retVal.AppendLine("");
            retVal.AppendLine(ex.Message);
            retVal.AppendLine("");

            var typeErrors = GetTypeQueryTyperErrorDetails<T>(reader);
            if (!String.IsNullOrWhiteSpace(typeErrors))
            {
                retVal.AppendLine("-----------------------------------------------");
                retVal.AppendLine("  The query return the following information  :");
                retVal.AppendLine("-----------------------------------------------");

                retVal.AppendLine(typeErrors);
            }

            retVal.AppendLine("-----------------------------------------------");
            return retVal.ToString();
        }

        internal static string GetTypeQueryTyperErrorDetails<T>(IDataReader reader)
        {
            var retVal = new StringBuilder();

            var properties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new { Name = p.Name, Setter = p.GetSetMethod(), Type = p.PropertyType })
                .Where(info => info.Setter != null)
                .ToList();


            var names = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                names.Add(reader.GetName(i));
                var readerType = reader.GetFieldType(i);

                var dbInformation = String.Format("Field name : {0} - type: {1} (dbType: {2})",
                                                  reader.GetName(i),
                                                  reader.GetFieldType(i),
                                                  reader.GetDataTypeName(i)
                    );
                var propInfo = "";
                var info = properties.FirstOrDefault(p => p.Name.ToUpper() == reader.GetName(i).ToUpper());


                if (info != null)
                {
                    if (!info.Type.IsAssignableFrom(readerType))
                    {
                        propInfo = String.Format("Prop Name  : {0} - type: {1}", info.Name, info.Type);
                    }
                }
                else
                    propInfo = String.Format("No Property Found.");

                if (!String.IsNullOrWhiteSpace(propInfo))
                {
                    retVal.AppendLine("");
                    retVal.AppendLine(String.Format("{0}", dbInformation));
                    retVal.AppendLine(String.Format("{0}", propInfo));

                }


            }
            return retVal.ToString();
        }


        #region helper
        private static void EmitInt32(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1: il.Emit(OpCodes.Ldc_I4_M1); break;
                case 0: il.Emit(OpCodes.Ldc_I4_0); break;
                case 1: il.Emit(OpCodes.Ldc_I4_1); break;
                case 2: il.Emit(OpCodes.Ldc_I4_2); break;
                case 3: il.Emit(OpCodes.Ldc_I4_3); break;
                case 4: il.Emit(OpCodes.Ldc_I4_4); break;
                case 5: il.Emit(OpCodes.Ldc_I4_5); break;
                case 6: il.Emit(OpCodes.Ldc_I4_6); break;
                case 7: il.Emit(OpCodes.Ldc_I4_7); break;
                case 8: il.Emit(OpCodes.Ldc_I4_8); break;
                default: il.Emit(OpCodes.Ldc_I4, value); break;
            }
        }

        #endregion

    }
}
