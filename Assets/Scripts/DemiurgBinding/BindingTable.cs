using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace DemiurgBinding
{
    public class BindingTable : ITable
    {
        public Table Table { get; set; }

        public string Name { get; set; }

        public BindingTable (Table table)
        {
            this.Table = table;
        }

        public IEnumerable GetKeys ()
        {
            List<object> keys = new List<object> ();
            foreach (var key in Table.Keys)
                keys.Add (key.ToObject ());
            return keys;
        }

        object ITable.Get (object id)
        {
            
            object obj = Table [id];
            /*if (obj is DynValue)
            {
                DynValue value = obj as DynValue;
                switch (value.Type)
                {
                case DataType.Boolean:
                    return value.CastToBool ();
                case DataType.Nil:
                    return null;
                case DataType.Function:
                    return new BindingFunction (value.Function);
                case DataType.Number:
                    return value.CastToNumber ();
                case DataType.String:
                    return value.CastToString ();
                case DataType.Table:
                    return new BindingTable (value.Table);
                default:
                    return null;
                }
            }*/
                
            if (obj is Table)
                return new BindingTable (obj as Table);
            if (obj is Closure)
                return new BindingFunction (obj as Closure);
            return obj;
        }

        void ITable.Set (object id, object o)
        {
            if (o is BindingTable)
                Table [id] = ((BindingTable)o).Table;
            else
                Table [id] = o;
        }
    }

}