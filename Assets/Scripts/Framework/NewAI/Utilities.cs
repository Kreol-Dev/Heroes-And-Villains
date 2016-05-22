using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace NewAI
{
	public class Utilities : MonoBehaviour
	{


		Dictionary<Type, List<Condition>> conditions = new Dictionary<Type, List<Condition>> ();
		Agent agent;

		void Awake ()
		{
			agent = GetComponent<Agent> ();
		}

		public float GetUtility (Promise promise)
		{
			List<Condition> promisedConditions = null;
			conditions.TryGetValue (promise.ConditionType, out promisedConditions);
			if (promisedConditions == null || promisedConditions.Count == 0)
				return 0f;
			float sumUt = 0f;
			for (int i = 0; i < promisedConditions.Count; i++)
			{
				if (promise.Iteration <= promisedConditions [i].Iteration)
					sumUt += promise.CheckCondition (agent, promisedConditions [i]);
			}
			return sumUt;
		}

		public List<Condition> GetConditions (Type type)
		{
			List<Condition> list = null;
			conditions.TryGetValue (type, out list);
			//			if (list == null)
			//				return new List<Condition> ();
			return list;
		}

		public void AddUtility (Condition condition)
		{
			List<Condition> list = null;
			Type cType = condition.GetType ();
			if (!conditions.TryGetValue (cType, out list))
			{
				list = new List<Condition> ();
				conditions.Add (cType, list);
			}
			list.Add (condition);

		}

		public void RemoveUtility (Condition condition)
		{
			List<Condition> list = null;
			Type cType = condition.GetType ();
			if (conditions.TryGetValue (cType, out list))
				list.Remove (condition);

		}
	}
}