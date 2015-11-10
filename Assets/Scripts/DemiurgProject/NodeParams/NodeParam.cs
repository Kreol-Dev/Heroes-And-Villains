
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
namespace Demiurg
{
    public abstract class NodeParam
    {
        public bool Undefined { get; internal set; }
        public string Name { get; internal set; }
        public NodeParam (string name)
        {
            Name = name;
            Undefined = false;
        }
		
        public abstract void GetItself (Table table);
        public abstract void GetItselfFrom (object o);
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





