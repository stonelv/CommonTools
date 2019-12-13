using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace CommonTools
{
    public class SqlGenerator<T> where T : new()
    {
        Hashtable args = new Hashtable();
        string table;

        /// <summary>
        /// Constructs Insert object
        /// </summary>
        /// <param name="table">table name to insert to</param>
        public SqlGenerator(T t)
        {
            this.table = t.GetType().Name;
            ReadKeyValuesFromClass(t);
        }

        private void ReadKeyValuesFromClass(T t)
        {
            foreach (var p in t.GetType().GetProperties())
            {
                var value = p.GetValue(t, null);
                var typeCode = Type.GetTypeCode(p.PropertyType);
                switch (typeCode)
                {
                    case TypeCode.String:
                    case TypeCode.DateTime:
                        value = value == null ? string.Empty : value;
                        value = "'" + value + "'";
                        break;
                    case TypeCode.Int32:
                        value = value == null ? 0 : value;
                        break;
                    case TypeCode.Decimal:
                        value = value == null ? 0.00d : value;
                        break;
                    default:
                        value = value == null ? string.Empty : value;
                        value = "'" + value + "'";
                        break;
                }
                Add(p.Name, value);
            }
        }

        /// <summary>
        /// Adds item to Insert object
        /// </summary>
        /// <param name="name">item name</param>
        /// <param name="val">item value</param>
        public void Add(string name, object val)
        {
            args.Add(name, val);
        }

        /// <summary>
        /// Removes item from Insert object
        /// </summary>
        /// <param name="name">item name</param>
        public void Remove(string name)
        {
            try
            {
                args.Remove(name);
            }
            catch
            {
                throw (new Exception("No such item"));
            }
        }

        /// <summary>
        /// Test representatnion of the Insert object (SQL query)
        /// </summary>
        /// <returns>System.String</returns>
        public string generateInsertSql()
        {
            StringBuilder s1 = new StringBuilder();
            StringBuilder s2 = new StringBuilder();

            IDictionaryEnumerator enumInterface = args.GetEnumerator();
            bool first = true;
            while (enumInterface.MoveNext())
            {
                if (first) first = false;
                else
                {
                    s1.Append(", ");
                    s2.Append(", ");
                }
                s1.Append(enumInterface.Key.ToString());
                s2.Append(enumInterface.Value.ToString());
            }

            return "INSERT INTO " + table + " (" + s1 + ") VALUES (" + s2 + ");";
        }

        //todo:改成sql参数化版本
        public SqlParameter[] generateSqlParameters()
        {
            return null;
        }

        /// <summary>
        /// Test representatnion of the Update object (SQL query)
        /// </summary>
        /// <returns>System.String</returns>
        public string generateUpdateSql(string fieldName)
        {
            StringBuilder s1 = new StringBuilder();
            StringBuilder s2 = new StringBuilder();

            IDictionaryEnumerator enumInterface = args.GetEnumerator();
            bool first = true;
            while (enumInterface.MoveNext())
            {
                if (first) first = false;
                else
                {
                    if (enumInterface.Key.ToString().Equals(fieldName)) //查询字段，拼接后用于where条件
                    {
                        s2.Append(fieldName);
                        s2.Append("=");
                        s2.Append(enumInterface.Value.ToString());
                    }
                    else
                    {
                        s1.Append(enumInterface.Key.ToString());
                        s1.Append("=");
                        s1.Append(enumInterface.Value.ToString());
                        s1.Append(",");
                    }

                }
            }

            return "UPDATE " + table + " set " + s1.ToString().Trim(',') + " where " + s2.ToString();
        }

        /// <summary>
        /// Gets or sets item into Insert object
        /// </summary>
        object this[string key]
        {
            get
            {
                Debug.Assert(args.Contains(key), "Key not found");
                return args[key];
            }
            set { args[key] = value; }
        }
    }
    
}
