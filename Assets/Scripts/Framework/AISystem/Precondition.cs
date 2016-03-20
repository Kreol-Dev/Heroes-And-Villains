using UnityEngine;
using System.Collections;

namespace AI
{
	public abstract class Precondition
	{
	}

	public abstract class Precondition<TComponent, TValue> : Precondition where TComponent : MonoBehaviour
	{
		public TComponent TargetComponent { get; internal set; }

		public abstract TValue TargetValue { get; }

		public abstract TValue CurrentValue { get; }

		//Either target value should be somehow lower, higher or the same.
		public abstract int Gradient { get; }

		public bool CheckGO (GameObject go)
		{
			return go.GetComponent<TComponent> () != null;
		}

		public void AssignTo (GameObject go)
		{
			TargetComponent = go.GetComponent<TComponent> ();
		}


	}


	public abstract class Postcondition
	{
		
	}

	//	public abstract class Postcondition<TComponent, TValue, TPrecondition> : Postcondition
	//		where TPrecondition : Precondition<TComponent, TValue>
	//	{
	//
	//	}
}


