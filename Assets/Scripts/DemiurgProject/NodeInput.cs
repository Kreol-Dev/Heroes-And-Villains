using UnityEngine;
using System.Collections;
using Signals;
using System;

namespace Demiurg
{
    public abstract class NodeInput
    {
        public abstract void ConnectTo (CreationNode node, string outputName);
    }
    public class NodeInput<T> : NodeInput
    {
        public T Content;
        public string Name { get; internal set; }
        Signal finishSignal = new Signal ();
        CreationNode node;
        public NodeInput (string name, Action onFinish, CreationNode node)
        {
            Name = name;
            this.node = node;
            finishSignal.AddOnce (onFinish);
        }
		
        public override void ConnectTo (CreationNode node, string outputName)
        {
            NodeOutput<T> output = node.GetOutput<T> (outputName);
            output.OnFinish (content => {
                Content = content;
                Debug.LogFormat ("Input {0} ({1}) received data from {2} ({3})", Name, this.node.Name, output.Name, output.NodeName);
                finishSignal.Dispatch ();});
        }
    }
}


