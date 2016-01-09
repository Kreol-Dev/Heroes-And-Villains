using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using MoonSharp.Interpreter;

namespace DemiurgBinding
{
    public class BindingFunction : ICallback
    {
        public Closure Closure { get; internal set; }

        public BindingFunction (Closure closure)
        {
            this.Closure = closure;
        }

        public object Call (params object[] args)
        {
            DynValue value = Closure.Call (args);
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
        }



    }

}