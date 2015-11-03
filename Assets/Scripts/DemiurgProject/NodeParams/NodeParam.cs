
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
namespace Demiurg
{
    public abstract class NodeParam
    {
        protected string Name;
        public NodeParam (string name)
        {
            Name = name;
        }
		
        public abstract void GetItself (Table table);
    }
    public abstract class NodeParam<T> : NodeParam
    {
        public T Content;
        public NodeParam (string name) : base(name)
        {
        }
        public static implicit operator T (NodeParam<T> param)
        {
            return param.Content;
        }

    }
}





