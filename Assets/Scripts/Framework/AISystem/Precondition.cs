using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AI
{
	public abstract class Precondition
	{
		public event EventHandler OnSatisfacton;
		public event EventHandler OnDissatisfaction;

		List<BaseAction> actions = new List<BaseAction> ();

		public void AttachAction (BaseAction action)
		{
			actions.Add (action);
		}

		public abstract bool IsPossibleFor (GameObject go);


	}
}


