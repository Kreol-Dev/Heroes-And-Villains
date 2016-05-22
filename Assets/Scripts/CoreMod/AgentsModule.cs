using UnityEngine;
using System.Collections;
using Demiurg.Core;
using System;
using System.Collections.Generic;

namespace CoreMod
{
	public class AgentsModule : Demiurg.Core.Avatar
	{
		[AConfig ("agents_count")]
		int agentsCount;
		[AConfig ("agent_namespace")]
		string agentNamespace;
		[AConfig ("agent_name")]
		string agentName;
		[AConfig ("agent_points")]
		int agentPoints;
		[AOutput ("finish_token")]
		object token = new object ();
		Stack<IGenerationAgent> agents = new Stack<IGenerationAgent> ();

		public override void Work ()
		{
			var objectsRoot = Find.Root<ObjectsRoot> ();
			var handle = objectsRoot.GetNamespace (agentNamespace).GetByName (agentName);


			for (int i = 0; i < agentsCount; i++)
			{
				GameObject go = handle.CreateObject ();
				go.name = agentName + i;
				EntityComponent[] entComponents = go.GetComponents<EntityComponent> ();
				for (int j = 0; j < entComponents.Length; j++)
					entComponents [j].PostCreate ();
				agents.Push (go.GetComponent<IGenerationAgent> ());
			}

			agents.Pop ().Generate (agentPoints, OnAgentFinish);
		}

		void OnAgentFinish ()
		{
			if (agents.Count == 0)
				FinishWork ();
			else
				agents.Pop ().Generate (agentPoints, OnAgentFinish);
		}
	}

	public interface IGenerationAgent
	{
		void Generate (int points, Action onAgentFinish);
	}


}


