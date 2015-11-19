using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using Demiurg.Core.Extensions;

namespace DemiurgBinding
{
    public class BindingTable : Table, ITable
    {
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



    }

}