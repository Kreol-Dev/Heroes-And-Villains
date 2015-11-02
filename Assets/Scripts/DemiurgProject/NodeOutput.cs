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
		public NodeOutput(string name)
		{
			Name = name;
			finishSignal.AddOnce(x => {Debug.Log("Finished output: "  + name);});
		}

		public void Finish(T content)
		{
			Content = content;
			finishSignal.Dispatch(Content);
		}

		public void Finish()
		{
			finishSignal.Dispatch(Content);
		}
		
		Signal<T> finishSignal = new Signal<T>();
		public void OnFinish(Action<T> onFinish)
		{
			finishSignal.AddOnce(onFinish);
		}
	}
}


