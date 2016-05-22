using UnityEngine;
using System.Collections;
using System;

namespace NewAI
{
	[Serializable]
	public abstract class Condition
	{
		public int Iteration;

		public float Utility;

		public abstract GameObject TargetAgent { get; }

		public abstract bool Satisfied { get; }

		public abstract void Setup (GameObject target);

		public abstract Task CreateTask (Agent agent);

		public void Free (Agent agent)
		{
			agent.RemoveCondition (this);
		}

		public Task AssignedTask;

		public string Data;
	}


}