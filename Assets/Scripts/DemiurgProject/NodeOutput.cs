using UnityEngine;
using System.Collections;
using System;
using Signals;
namespace Demiurg
{
    public class NodeOutput<T>
    {
        public T Content;
        public string Name { get; internal set; }
        public string NodeName { get { return node.Name; } }
        CreationNode node;
        public NodeOutput (string name, CreationNode node)
        {
            Name = name;
            this.node = node;
            finishSignal.AddOnce (x => {
                Debug.LogFormat ("Finished output: {0} in {1}", name, node.Name);});
        }

        public void Finish (T content)
        {
            Content = content;
            finishSignal.Dispatch (Content);
        }

        public void Finish ()
        {
            finishSignal.Dispatch (Content);
        }
		
        Signal<T> finishSignal = new Signal<T> ();
        public void OnFinish (Action<T> onFinish)
        {
            finishSignal.AddOnce (onFinish);
        }
    }

}


