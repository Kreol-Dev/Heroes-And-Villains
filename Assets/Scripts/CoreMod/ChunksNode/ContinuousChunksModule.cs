
using System.Collections;
using System.Collections.Generic;
using Demiurg;
using UnityEngine;


namespace CoreMod
{
    public partial class ContinuousChunksModule : CreationNode
    {

        NodeInput<int[,]> mainI;
        NodeOutput<int[,]> assignmentsO;
        NodeOutput<List<GameObject>> chunksO;
        StringParam planetConnectivity;
        protected override void SetupIOP ()
        {
            mainI = Input<int[,]> ("main");
            assignmentsO = Output<int[,]> ("assignments");
            chunksO = Output<List<GameObject>> ("chunks");
            planetConnectivity = Config<StringParam> ("planet_connectivity");
        }
        protected override void Work ()
        {
            AgentEnvironment env = new AgentEnvironment (mainI.Content, TilesConnection.Four, EnvConnection.Sphere);
            env.NewAgent (0);
            //env.Update ();
            //for (int i = 0; i < 10; i++)
            //    env.Update ();
            int iters = 0;
            while (env.Active)
            {
                iters++;
                env.Update ();
            }
            Debug.LogFormat ("Chunks fromed in {0} iterations", iters);
            int sizeX = mainI.Content.GetLength (0);
            int sizeY = mainI.Content.GetLength (1);
            int[,] outputAssignments = new int[sizeX, sizeY];
            for (int i = 0; i < env.Assigned.Length; i++)
                outputAssignments [i % sizeX, i / sizeX] = env.Assigned [i];
            assignmentsO.Finish (outputAssignments);

            List<GameObject> objects = new List<GameObject> ();
            for (int i = 0; i < env.Agents.Count; i++)
            {
                ChunkAgent agent = env.Agents [i];
                GameObject obj = new GameObject ("Chunk GO");
                ChunkSlot slot = obj.AddComponent<ChunkSlot> ();
                slot.Tiles = new TileRef[agent.Tiles.Count];
                for (int j = 0; j < agent.Tiles.Count; j++)
                    slot.Tiles [j] = new TileRef (){X = agent.Tiles[j] % sizeX, Y = agent.Tiles[j] / sizeX};
                slot.ID = agent.ID;
                slot.Surface = agent.Surface;
                objects.Add (obj);
            }
            chunksO.Finish (objects);
        }
       
    }

   



}





