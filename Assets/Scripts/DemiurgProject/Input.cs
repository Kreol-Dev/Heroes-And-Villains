
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Signals;


namespace Demiurg
{
	public abstract class Input : IDependency
	{
		Signal signal = new Signal();
		public Signal Fulfill { get { return signal; }}
		protected Scribe Scribe;
		public Input()
		{
			Scribe = Scribes.Find("InputScribe");
		}
		public abstract void ConnectTo(Output output);
	}

	public class Input<T> : Input
	{
		public T Content;
		Output<T> cachedOutput;
		public override sealed void ConnectTo(Output output)
		{
			Output<T> targetOutput = output as Output<T>;
			if (targetOutput == null)
			{
				Scribe.LogFormat("Output type {0} doesn't match input type {1}", output.GetType(), this.GetType());
				return;
			}
			cachedOutput = targetOutput;
			cachedOutput.Fulfill.AddListener(() => { Content = cachedOutput; });
		}
	}
}



