using UnityEngine;
using System.Collections;
using System;

namespace NewAI
{
	public abstract class Promise
	{
		public abstract Type ConditionType { get; }

		public int Iteration;
		//public Agent Agent;
		public abstract float CheckCondition (Agent agent, Condition c);
	}

	public abstract class Promise<C> : Promise where C : Condition
	{
		public override Type ConditionType {
			get
			{
				return typeof(C);
			}
		}

		public sealed override float CheckCondition (Agent agent, Condition c)
		{
			return CheckCondition (agent, c as C);
		}

		public abstract float CheckCondition (Agent agent, C condition);
	}



}