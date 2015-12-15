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

        public IEnumerable<object> GetKeys ()
        {
            return (IEnumerable<object>)Table.Keys;
        }

        object ITable.Get (object id)
        {
            
            object obj = Table [id];
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