using UnityEngine;
using System.Collections;

namespace NewAI
{
	public abstract class Action
	{
		
		public abstract void Update (System.Action onSuccess, System.Action onFail, Utilities uts);

		public abstract float GetUtility (Utilities uts);

		public abstract void Setup (Agent agent);
		//Here check for conditions and create tasks. If conditions became unsatisfied after that - action failed
		public abstract void Prepare (int iteration);

		public abstract bool IsPossibleToPerformBy (GameObject go);
	}
}



