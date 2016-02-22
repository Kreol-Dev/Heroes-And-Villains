using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace AI
{
	public class ActionGraph
	{
		Dictionary<Type, GoalDefinition> Goals;
		Dictionary<Type, ConditionDefinition> Conditions;
		Dictionary<Type, ActionDefition> Actions;

		public void RegisterGoal (Type goal)
		{
			
		}

		public void RegisterCondition (Type condition)
		{
			
		}

		public void RegisterAction (Type action)
		{
			
		}

		//public
	}


	public class ActionDefition
	{
		public Type Type;
		public List<ConditionDefinition> Conditions;
		public ConditionDefinition ResultCondition;
	}

	public class ConditionDefinition
	{
		public Type Type;
		public List<ActionDefition> Actions;
	}

	public class GoalDefinition
	{
		public Type Type;
		public List<ConditionDefinition> Conditions;
	}
}


