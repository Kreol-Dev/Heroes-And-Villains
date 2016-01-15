
using System.Collections;
using System.Collections.Generic;
using Demiurg;
using UnityEngine;
using Demiurg.Core;


namespace CoreMod
{
	public partial class ContinuousChunksModule : Demiurg.Core.Avatar
	{
		[AInput ("main")]
		int[,] mainI;
		[AOutput ("assignments")]
		int[,] assignmentsO;
		[AOutput ("chunks")]
		List<GameObject> chunksO;
		[AConfig ("planet_connectivity")]
		string planetConnectivity;

		public override void Work ()
		{
			EnvConnection envConnection = EnvConnection.None;
			if (planetConnectivity == "cylinder")
				envConnection = EnvConnection.Cylinder;
			else if (planetConnectivity == "sphere")
				envConnection = EnvConnection.Sphere;
			AgentEnvironment env = new AgentEnvironment (mainI, TilesConnection.Four, envConnection);
			env.NewAgent (0);
			int iters = 0;
			while (env.Active) {
				iters++;
				env.Update ();
			}
			Debug.LogFormat ("Chunks fromed in {0} iterations", iters);
			int sizeX = mainI.GetLength (0);
			int sizeY = mainI.GetLength (1);
			int[,] outputAssignments = new int[sizeX, sizeY];
			for (int i = 0; i < env.Assigned.Length; i++)
				outputAssignments [i % sizeX, i / sizeX] = env.Assigned [i];
			assignmentsO = outputAssignments;
			List<GameObject> objects = new List<GameObject> ();
			var map = Find.Root<TilesRoot> ().MapHandle;
			for (int i = 0; i < env.Agents.Count; i++) {
				ChunkAgent agent = env.Agents [i];
				GameObject obj = new GameObject ("Chunk GO");
				ChunkSlot slot = obj.AddComponent<ChunkSlot> ();
				slot.Tiles = new TileHandle[agent.Tiles.Count];
				for (int j = 0; j < agent.Tiles.Count; j++)
					slot.Tiles [j] = map.GetHandle (agent.Tiles [j] % sizeX, agent.Tiles [j] / sizeX);
				slot.ID = agent.ID;
				slot.Surface = agent.Surface;
				objects.Add (obj);
			}
			chunksO = objects;
			FinishWork ();
		}
       
	}

   



}





