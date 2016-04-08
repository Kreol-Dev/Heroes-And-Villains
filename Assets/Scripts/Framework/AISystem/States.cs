using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AI
{
	public class States : Root
	{
		protected override void CustomSetup ()
		{
			Fulfill.Dispatch ();
		}

		public delegate void ObjectDelegate (object o);

		public event ObjectDelegate StateChanged;

		public void Notify_StateChanged (object o)
		{
			if (StateChanged != null)
				StateChanged (o);
		}
	}


	public interface IState<T>
	{
		T Value { get; set; }
	}


	public class StateStack<S, T, C> where S : IState<T> where C : Condition<S, T>
	{
		public bool RequestsStack;
		List<object> stack = new List<object> ();

		//C >= 25
		//C >= 30 - second
		//C <= 35 - first
		//C <= 40
		//---------
		//C >= 25
		//C >= 30 - second
		//C <= 40
		//C <= 35 - first
		//C >= 25
		C firstCondition = null;
		C secondCondition = null;

		public void Push (Promise<S, T, C> promise)
		{
			stack.Add (promise);
			//if ()

		}

		public bool Push (C condition)
		{
			
			stack.Add (condition);
			return false;
		}



	}



	public class Promise<S, T, C> where S : IState<T> where C : Condition<S, T>
	{
		public C GetRequest (C condition)
		{
			return null;
		}
	}

	public abstract class Condition<S, T> where S : IState<T>
	{
		public bool Request;

		public abstract bool Satisfied (S state);
	}

	public class Building
	{
		//Effect_ChangeBaseProduction
		// or
		//Effect_ChangeBaseFood
		// or
		//Effect_ChangeProdMod
		// or
		//Effect_ChangeFoodMod
	}

	public class Action_Build
	{
		//Promise_OccupySlot
		//Promise_ChangeMoney

		//Condition_HaveMoney
		//Condition_HaveSlot

		//public Building building; - can extract promise from an effect
		public void PushTo (Plan plan)
		{
			//plan.PushPromise(Promises.Get(building.effect))
		}

		public void PopFrom (Plan plan)
		{
			
		}
	}

	public class Task_Build
	{
		//Promise_MakeTrue

		public void PushTo (Plan plan)
		{
			//Запихивает туда своё обещание, а потом ещё и все свои требования, 
			//которые позже будут переданы другим Task'ам, если таковые будут
		}

		public void PopFrom (Plan plan)
		{
			
		}
	}

}