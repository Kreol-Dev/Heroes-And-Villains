
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
        NodeOutput<Dictionary<int, List<int>>> chunksO;
        StringParam planetConnectivity;
        protected override void SetupIOP ()
        {
            mainI = Input<int[,]> ("main");
            assignmentsO = Output<int[,]> ("assignments");
            chunksO = Output<Dictionary<int, List<int>>> ("chunks");
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

            chunksO.Content = new Dictionary<int, List<int>> ();
            var chunks = chunksO.Content;
            for (int i = 0; i < env.Agents.Count; i++)
                chunks.Add (env.Agents [i].ID, env.Agents [i].Tiles);
            chunksO.Finish (chunks);
        }
        #region enums
        enum TilesConnection
        {
            Four,
            Eight
        }
        enum EnvConnection
        {
            None,
            Cylinder,
            Sphere
        }
        enum Direction
        {
            Left,
            Right,
            Top,
            Down,
            TopLeft,
            TopRight,
            DownLeft,
            DownRight
        }
        #endregion
    }

   



}





