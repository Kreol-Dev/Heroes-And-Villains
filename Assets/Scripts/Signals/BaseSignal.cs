using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Signals
{
	public class BaseSignal
	{
		public event Action<BaseSignal, object[]> BaseListener = delegate { };
		public event Action<BaseSignal, object[]> OnceBaseListener = delegate { };
		
		public void Dispatch(object[] args) 
		{ 
			BaseListener(this, args);
			OnceBaseListener(this, args);
			OnceBaseListener = delegate { };
		}
		
		public virtual void AddListener(Action<BaseSignal, object[]> callback) 
		{
			foreach (Delegate del in BaseListener.GetInvocationList())
			{
				Action<BaseSignal, object[]> action = (Action<BaseSignal, object[]>)del;
				if (callback.Equals(action)) //If this callback exists already, ignore this addlistener
					return;
			}
			BaseListener += callback;
		}
		
		public virtual void AddOnce(Action<BaseSignal, object[]> callback)
		{
			foreach (Delegate del in OnceBaseListener.GetInvocationList())
			{
				Action<BaseSignal, object[]> action = (Action<BaseSignal, object[]>)del;
				if (callback.Equals(action)) //If this callback exists already, ignore this addlistener
					return;
			}
			OnceBaseListener += callback;
		}
		
		public virtual void RemoveListener(Action<BaseSignal, object[]> callback) { BaseListener -= callback; }
		
		
	}
}