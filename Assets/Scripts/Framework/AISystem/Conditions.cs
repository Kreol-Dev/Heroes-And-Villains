using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AI
{
	public class Conditions : MonoBehaviour
	{
		Dictionary<Type, Condition> conditions = new Dictionary<Type, Condition> ();

		public T GetCondition<T> () where T : Condition
		{
			Type t = typeof(T);
			Condition condition = null;
			conditions.TryGetValue (t, out condition);
			return condition as T;
		}

		//		public void AssignCondition (Type conditionType)
		//		{
		//			conditions.Add(conditionType,
		//		}
	}
}


