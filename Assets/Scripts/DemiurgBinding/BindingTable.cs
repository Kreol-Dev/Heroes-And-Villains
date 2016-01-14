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
            /*cachedValue = Table.Get (id);
            if (cachedValue == null)
            {
                Debug.LogFormat ("id {0} returned null", id);
                return null;
            }
            Debug.LogFormat ("id {0} returned value {1} with type {2}", id, cachedValue, cachedValue.Type);
                
            switch (cachedValue.Type)
            {
            case DataType.Boolean:
                return cachedValue.CastToBool ();
            case DataType.Nil:
                return null;
            case DataType.Function:
                return new BindingFunction (cachedValue.Function);
            case DataType.Number:
                return cachedValue.CastToNumber ();
            case DataType.String:
                return cachedValue.CastToString ();
            case DataType.Table:
                return new BindingTable (cachedValue.Table);
            default:
                return null;
            }
            return cachedValue;*/
            object obj = Table [id];
            if (obj == null)
            {
                Debug.LogFormat ("key {0} value NULL", id);
                return null;

            }
            Table table = obj as Table;

            if (table != null)
                return new BindingTable (table);
            else
            {
                Closure closure = obj as Closure;
                if (closure != null)
                    return new BindingFunction (closure);
            }

            Debug.LogFormat ("key {0} value {1} type {2}", id, obj, obj.GetType ());
            return obj;
        }

        void ITable.Set (object id, object o)
        {
            if (o is BindingTable)
                Table [id] = ((BindingTable)o).Table;
            else
            if (o is BindingFunction)
                Table [id] = ((BindingFunction)o).Closure;
            else
                Table [id] = o;
        }
    }

}