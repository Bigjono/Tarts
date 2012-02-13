using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Bronson.DB.Common.QueryProcessor.Mapper.Model;

namespace Bronson.DB.Common.QueryProcessor.Mapper
{
    public static class ParametersHelper
    {

        #region helper dictionaries
        private static readonly ConcurrentDictionary<Type, Func<object, List<ParamInfo>>> cachedParamReaders = new ConcurrentDictionary<Type, Func<object, List<ParamInfo>>>();
        private static readonly Dictionary<Type, DbType> typeMap;
        #endregion


        #region db type lookups

        static ParametersHelper()
        {
            
            typeMap = new Dictionary<Type, DbType>();
            typeMap[typeof(byte)] = DbType.Byte;
            typeMap[typeof(sbyte)] = DbType.SByte;
            typeMap[typeof(short)] = DbType.Int16;
            typeMap[typeof(ushort)] = DbType.UInt16;
            typeMap[typeof(int)] = DbType.Int32;
            typeMap[typeof(uint)] = DbType.UInt32;
            typeMap[typeof(long)] = DbType.Int64;
            typeMap[typeof(ulong)] = DbType.UInt64;
            typeMap[typeof(float)] = DbType.Single;
            typeMap[typeof(double)] = DbType.Double;
            typeMap[typeof(decimal)] = DbType.Decimal;
            typeMap[typeof(bool)] = DbType.Boolean;
            typeMap[typeof(string)] = DbType.String;
            typeMap[typeof(char)] = DbType.StringFixedLength;
            typeMap[typeof(Guid)] = DbType.Guid;
            typeMap[typeof(DateTime)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            typeMap[typeof(byte[])] = DbType.Binary;

            typeMap[typeof(byte?)] = DbType.Byte;
            typeMap[typeof(sbyte?)] = DbType.SByte;
            typeMap[typeof(short?)] = DbType.Int16;
            typeMap[typeof(ushort?)] = DbType.UInt16;
            typeMap[typeof(int?)] = DbType.Int32;
            typeMap[typeof(uint?)] = DbType.UInt32;
            typeMap[typeof(long?)] = DbType.Int64;
            typeMap[typeof(ulong?)] = DbType.UInt64;
            typeMap[typeof(float?)] = DbType.Single;
            typeMap[typeof(double?)] = DbType.Double;
            typeMap[typeof(decimal?)] = DbType.Decimal;
            typeMap[typeof(bool?)] = DbType.Boolean;
            typeMap[typeof(char?)] = DbType.StringFixedLength;
            typeMap[typeof(Guid?)] = DbType.Guid;
            typeMap[typeof(DateTime?)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset?)] = DbType.DateTimeOffset;

        }

        private static DbType LookupDbType(Type type)
        {
            DbType dbType;
            if (typeMap.TryGetValue(type, out dbType))
            {
                return dbType;
            }
            else
            {
                if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    // use xml to denote its a list, hacky but will work on any DB
                    return DbType.Xml;
                }
            }

            throw new NotSupportedException("The type : " + type.ToString() + " is not supported by the Mapper");
        }

        public static class DynamicStub
        {
            public static Type Type = typeof(DynamicStub);
        }

        #endregion

        #region parameter convertors

        public static List<ParamInfo> GetParamInfo(object param)
        {
            Func<object, List<ParamInfo>> paramInfoGenerator;
            List<ParamInfo> paramInfo = null;

            if (param != null)
            {
                if (!cachedParamReaders.TryGetValue(param.GetType(), out paramInfoGenerator))
                {
                    paramInfoGenerator = CreateParamInfoGenerator(param.GetType());
                    cachedParamReaders[param.GetType()] = paramInfoGenerator;
                }
                paramInfo = paramInfoGenerator(param);
            }

            return paramInfo;
        }

        public static string GetParamInfoToString(object param)
        {
            if (param == null) return "";
            var retVal = new StringBuilder();
            var parameters = ParametersHelper.GetParamInfo(param);
            foreach (var parm in parameters)
            {
                retVal.AppendFormat("{0}={1}", parm.Name, parm.Val);
            }
            return retVal.ToString();
        }

        private static Func<object, List<ParamInfo>> CreateParamInfoGenerator(Type type)
        {
            DynamicMethod dm = new DynamicMethod("ParamInfo" + Guid.NewGuid().ToString(), typeof(List<ParamInfo>), new Type[] { typeof(object) }, true);

            var il = dm.GetILGenerator();

            il.DeclareLocal(type); // 0
            il.Emit(OpCodes.Ldarg_0); // stack is now [untyped-param]
            il.Emit(OpCodes.Unbox_Any, type); // stack is now [typed-param]
            il.Emit(OpCodes.Stloc_0);// stack is now empty

            il.Emit(OpCodes.Newobj, typeof(List<ParamInfo>).GetConstructor(Type.EmptyTypes)); // stack is now [list]

            foreach (var prop in type.GetProperties().OrderBy(p => p.Name))
            {
                // we want to call list.Add(ParamInfo.Create(string name, DbType type, object val))

                il.Emit(OpCodes.Dup); // stack is now [list] [list]

                il.Emit(OpCodes.Ldstr, prop.Name); // stack is  [list] [list] [name]
                il.Emit(OpCodes.Ldc_I4, (int)LookupDbType(prop.PropertyType)); // stack is [list] [list] [name] [dbtype]
                il.Emit(OpCodes.Ldloc_0); // stack is [list] [list] [name] [dbtype] [typed-param]
                il.Emit(OpCodes.Callvirt, prop.GetGetMethod()); // stack is [list] [list] [name] [dbtype] [typed-value]
                if (prop.PropertyType.IsValueType)
                {
                    il.Emit(OpCodes.Box, prop.PropertyType); // stack is [list] [list] [name] [dbtype] [untyped-value]
                }
                il.Emit(OpCodes.Call, typeof(ParamInfo).GetMethod("Create", BindingFlags.Static | BindingFlags.Public)); // stack is [list] [list] [param-info]
                il.Emit(OpCodes.Callvirt, typeof(List<ParamInfo>).GetMethod("Add", BindingFlags.Public | BindingFlags.Instance)); // stack is [list]
            }

            il.Emit(OpCodes.Ret);

            return (Func<object, List<ParamInfo>>)dm.CreateDelegate(typeof(Func<object, List<ParamInfo>>));
        }

        #endregion
    }
}
