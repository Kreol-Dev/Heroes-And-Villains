using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace AI
{
	public class ActionGraph
	{
		
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


