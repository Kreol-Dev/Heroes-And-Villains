using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using Demiurg.Core.Extensions;

namespace DemiurgBinding
{
    public class BindingTable : Table, ITable
    {
        public string Name { get; set; }
        public BindingTable (Script script):base(script)
        {

        }
        object ITable.Get (string id)
        {
            return this [id];
        }

        object ITable.Get (int id)
        {
            return this [id];
        }
        void ITable.Set (string id, object o)
        {
            this [id] = o;
        }
        void ITable.Set (int id, object o)
        {
            this [id] = o;
        }
    }

}