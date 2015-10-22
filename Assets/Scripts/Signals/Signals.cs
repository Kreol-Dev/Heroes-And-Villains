using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
namespace Signals
{
	public class Signal : BaseSignal
	{
		public event Action Listener = delegate { };
		public event Action OnceListener = delegate { };
		
		public void AddListener(Action callback)
		{
			Listener = this.AddUnique(Listener, callback);
		}
		
		public void AddOnce(Action callback)
		{
			OnceListener = this.AddUnique(OnceListener, callback);
		}
		public void RemoveListener(Action callback) { Listener -= callback; }
		public void Dispatch()
		{
			Listener();
			OnceListener();
			OnceListener = delegate { };
			base.Dispatch(null);
		}
		
		private Action AddUnique(Action listeners, Action callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				listeners += callback;
			}
			return listeners;
		}
	}
	
	/// Base concrete form for a Signal with one parameter
	public class Signal<T> : BaseSignal
	{
		public event Action<T> Listener = delegate { };
		public event Action<T> OnceListener = delegate { };
		
		public void AddListener(Action<T> callback)
		{
			//UnityEngine.Debug.Log("listener " + callback.GetInvocationList()[0].Target.GetType().ToString() + " " +  callback.GetInvocationList()[0].Method.Name);
			Listener = this.AddUnique(Listener, callback);
		}
		
		public void AddOnce(Action<T> callback)
		{
			//UnityEngine.Debug.Log("once " + callback.GetInvocationList()[0].Target.GetType().ToString());
			OnceListener = this.AddUnique(OnceListener, callback);
		}
		// UnityEngine.Debug.Log("REMOVED " + callback.GetInvocationList()[0].Target.GetType().ToString() + " " +  callback.GetInvocationList()[0].Method.Name);
		public void RemoveListener(Action<T> callback) {  Listener -= callback; }
		public void Dispatch(T type1)
		{
			//			foreach (var del in Listener.GetInvocationList())
			//			{
			//				if (del.Target != null)
			//					UnityEngine.Debug.Log("CALLED" + del.Target.GetType().ToString() + "  " + del.Method.Name);
			//			}
			Listener(type1);
			OnceListener(type1);
			OnceListener = delegate { };
			object[] outv = { type1 };
			base.Dispatch(outv);
		}
		
		private Action<T> AddUnique(Action<T> listeners, Action<T> callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				//				UnityEngine.Debug.Log("AddUnique " + callback.GetInvocationList()[0].Target.GetType().ToString() + " " +  callback.GetInvocationList()[0].Method.Name);
				listeners += callback;
				//				foreach (var del in listeners.GetInvocationList())
				//				{
				//					if (del.Target != null)
				//						UnityEngine.Debug.Log("ADDED" + del.Target.GetType().ToString() + "  " + del.Method.Name);
				//				}
			}
			return listeners;
		}
	}
	
	/// Base concrete form for a Signal with two parameters
	public class Signal<T, U> : BaseSignal
	{
		public event Action<T, U> Listener = delegate { };
		public event Action<T, U> OnceListener = delegate { };
		
		public void AddListener(Action<T, U> callback)
		{
			Listener = this.AddUnique(Listener, callback);
		}
		
		public void AddOnce(Action<T, U> callback)
		{
			OnceListener = this.AddUnique(OnceListener, callback);
		}
		
		public void RemoveListener(Action<T, U> callback) { Listener -= callback; }
		public void Dispatch(T type1, U type2)
		{
			Listener(type1, type2);
			OnceListener(type1, type2);
			OnceListener = delegate { };
			object[] outv = { type1, type2 };
			base.Dispatch(outv);
		}
		private Action<T, U> AddUnique(Action<T, U> listeners, Action<T, U> callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				listeners += callback;
			}
			return listeners;
		}
	}
	
	/// Base concrete form for a Signal with three parameters
	public class Signal<T, U, V> : BaseSignal
	{
		public event Action<T, U, V> Listener = delegate { };
		public event Action<T, U, V> OnceListener = delegate { };
		
		public void AddListener(Action<T, U, V> callback)
		{
			Listener = this.AddUnique(Listener, callback);
		}
		
		public void AddOnce(Action<T, U, V> callback)
		{
			OnceListener = this.AddUnique(OnceListener, callback);
		}
		
		public void RemoveListener(Action<T, U, V> callback) { Listener -= callback; }
		public void Dispatch(T type1, U type2, V type3)
		{
			Listener(type1, type2, type3);
			OnceListener(type1, type2, type3);
			OnceListener = delegate { };
			object[] outv = { type1, type2, type3 };
			base.Dispatch(outv);
		}
		private Action<T, U, V> AddUnique(Action<T, U, V> listeners, Action<T, U, V> callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				listeners += callback;
			}
			return listeners;
		}
	}
	
	/// Base concrete form for a Signal with four parameters
	public class Signal<T, U, V, W> : BaseSignal
	{
		public event Action<T, U, V, W> Listener = delegate { };
		public event Action<T, U, V, W> OnceListener = delegate { };
		
		public void AddListener(Action<T, U, V, W> callback)
		{
			Listener = this.AddUnique(Listener, callback);
		}
		
		public void AddOnce(Action<T, U, V, W> callback)
		{
			OnceListener = this.AddUnique(OnceListener, callback);
		}
		
		public void RemoveListener(Action<T, U, V, W> callback) { Listener -= callback; }
		public void Dispatch(T type1, U type2, V type3, W type4)
		{
			Listener(type1, type2, type3, type4);
			OnceListener(type1, type2, type3, type4);
			OnceListener = delegate { };
			object[] outv = { type1, type2, type3, type4 };
			base.Dispatch(outv);
		}
		
		private Action<T, U, V, W> AddUnique(Action<T, U, V, W> listeners, Action<T, U, V, W> callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				listeners += callback;
			}
			return listeners;
		}
	}
}

