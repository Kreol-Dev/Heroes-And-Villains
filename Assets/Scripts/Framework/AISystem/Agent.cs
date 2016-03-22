using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AI
{
	[ECompName ("agent")]
	public class Agent : EntityComponent
	{
		public override EntityComponent CopyTo (GameObject go)
		{
			var cmp = go.AddComponent<Agent> ();
			return cmp;
		}

		static Ticker ticker;

		public override void PostCreate ()
		{
			if (ticker == null)
				ticker = Find.Root<AI.Ticker> ();
		}

		protected override void PostDestroy ()
		{
		}

		[SerializeField]
		List<Action> actions = new List<Action> ();

		public void Perform (Condition condition)
		{
			//actions.Add (action);
			//ticker.Tick += action.OnTick;
		}

		void OnActionDone (Action action)
		{
			
		}

		void OnActionFailed (Action action)
		{
			
		}

		void Update ()
		{
			for (int i = 0; i < actions.Count; i++)
				actions [i].OnTimedUpdate (Time.deltaTime);
		}
	}
}


