using UnityEngine;
using System.Collections;

namespace AI
{


	public abstract class BaseAction
	{
		public abstract float ChanceOfSuccess { get; }

		public abstract float Difficulty { get; }

		public abstract void Do ();

		public bool IsPossible ()
		{
			
		}

		public bool IsAvailable ()
		{
			
		}
	}

	public abstract class Action<TPrecondition> : BaseAction where TPrecondition : Precondition
	{
		TPrecondition precondition;



		public void LeadsTo (TPrecondition precondition)
		{
			this.precondition = precondition;
			precondition.AttachAction (this);
		}


	}
}