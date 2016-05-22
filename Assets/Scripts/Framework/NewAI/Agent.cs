using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text;
using UnityEngine.UI;
using UIO;

namespace NewAI
{
	[AShared]
	[ECompName ("new_agent")]
	public class Agent : EntityComponent
	{
		public override EntityComponent CopyTo (GameObject go)
		{
			return go.AddComponent<Agent> ();
		}

		public override void PostCreate ()
		{
			Uts = GetComponent<Utilities> ();
			Debug.Log (gameObject.name + " start");
			//InvokeRepeating ("OnTick", 1f, 1f);
			Find.Root<AI.Ticker> ().Tick += OnTick;
		}

		protected override void PostDestroy ()
		{
			Find.Root<AI.Ticker> ().Tick -= OnTick;
		}

		public Utilities Uts { get; internal set; }

		public int iteration = 0;
		public float PenaltyPerUnsatisfiedCondition;
		public float PenaltyPerIteration;

		//public Stack<Task> TaskStack = new Stack<Task> ();
		public Condition curCondition = null;
		//public Action curAction = null;
		[SerializeField]
		List<Condition> conditions = new List<Condition> ();

		void OnTick ()
		{
			var txt = GetComponentInChildren<Text> ();
			txt.text = "";
			foreach (var c in conditions)
				txt.text += c + Environment.NewLine;
			Debug.Log (gameObject.name + " tick");
			if (curCondition == null)
			{
				if (conditions.Count == 0)
					return;
				//conditions.RemoveAll (c => c.Satisfied);
				Condition maxUt = conditions [0];
				for (int i = 0; i < conditions.Count; i++)
				{
					Condition c = conditions [i];
					if (c.AssignedTask == null)
						c.AssignedTask = c.CreateTask (this);
					if (c.Utility > maxUt.Utility)
						maxUt = c;
				}


				Debug.Log ("task please?");
				if (maxUt.AssignedTask.Do (this, maxUt, Uts))
				{

					Debug.Log ("new task");
					curCondition = maxUt;
				} else
				{

					Debug.Log ("no task");
					curCondition = null;
				}
			} else
			{
				Debug.Log ("do planned task");
				if (!curCondition.AssignedTask.Do (this, curCondition, Uts))
					curCondition = null;
				//curAction.Update (OnActionSuccess, OnActionFail);
			}

		}

		public List<Action> GetActions (Type jobType, Action alreadyChosen)
		{
			
			Type chosenType = null;
			if (alreadyChosen != null)
				chosenType = alreadyChosen.GetType ();
			List<Type> actionsTypes = null;
			actionsByJob.TryGetValue (jobType, out actionsTypes);
			List<Action> actions = new List<Action> ();
			for (int i = 0; i < actionsTypes.Count; i++)
			{
				if (actionsTypes [i] != chosenType)
					actions.Add (Activator.CreateInstance (actionsTypes [i]) as Action);
				else
					actions.Add (alreadyChosen);
			}
			return actions;
		}

		static Dictionary<Type, List<Type>> actionsByJob = new Dictionary<Type, List<Type>> ();

		[RuntimeInitializeOnLoadMethod]
		static void FindActions ()
		{
			Debug.Log ("Collect actions and jobs");
			var types = Find.Root<ModsManager> ().GetAllTypes ();
			foreach (var type in types)
			{
				if (type.IsSubclassOf (typeof(AI.Action)) && !type.IsGenericType && !type.IsAbstract)
				{
					var jobs = type.GetInterfaces ();
					foreach (var jobType in jobs)
					{
						List<Type> actionsList = null;
						if (!actionsByJob.TryGetValue (jobType, out actionsList))
						{
							actionsList = new List<Type> ();
							actionsByJob.Add (jobType, actionsList);
						}
						actionsList.Add (type);
					}

				}

			}

			StringBuilder builder = new StringBuilder ();
			foreach (var jobActions in actionsByJob)
			{
				builder.Append (jobActions.Key).Append (Environment.NewLine);

				foreach (var actionType in jobActions.Value)
					builder.Append ("   ").Append (actionType).Append (Environment.NewLine);
			}
			Debug.LogWarning (builder.ToString ());

		}

		public void AddCondition (Condition condition)
		{
			Uts.AddUtility (condition);
			conditions.Add (condition);
		}

		public void RemoveCondition (Condition condition)
		{
			conditions.Remove (condition);
			Uts.RemoveUtility (condition);
			if (curCondition == condition)
				curCondition = null;
		}




	}
}

