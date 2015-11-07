using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;


namespace CoreMod
{
    public partial class ContinuousChunksModule : CreationNode
    {
        class ChunkAgent
        {
            public bool Active { get; internal set; }
            public List<int> Tiles = new List<int> ();
            public List<int> Frontier = new List<int> ();
            int surface;
            public int ID { get; internal set; }
            AgentEnvironment env;
            public ChunkAgent (int id, int tile, AgentEnvironment env)
            {
                Active = true;
                ID = id;
                this.env = env;
                env.AssignID (tile, this);
                surface = env.Surface (tile);
                Frontier.Add (tile);
            }

            public void Update ()
            {
                List<int> newFrontier = new List<int> ();
                for (int i = 0; i < Frontier.Count; i++)
                {
                    newFrontier.AddRange (Expand (Frontier [i]));
                    Tiles.Add (Frontier [i]);
                }
                if (newFrontier.Count == 0)
                    Active = false;
                Frontier = newFrontier;
                
            }
            
            List<int> Expand (int tile)
            {
                List<int> frontier = new List<int> ();
                for (int i = 0; i < env.TileConnections.Length; i++)
                {
                    int nextTile = TryExpandTo (tile, env.TileConnections [i]);
                    if (nextTile != -1)
                    {
                        frontier.Add (nextTile);
                        env.AssignID (nextTile, this);
                    }
                }
                return frontier;
            }
            
            int TryExpandTo (int tile, Direction direction)
            {
                int nextTile = env.GetNext (tile, direction);
                int nextTileAssignment = env.Assignment (nextTile);
                int nextTileSurface = env.Surface (nextTile);
                if (nextTileAssignment == -1)
                {
                    //Debug.LogFormat ("{0} {1}", nextTileSurface, surface);
                    if (nextTileSurface == surface)
                        return nextTile;
                    env.NewAgent (nextTile);
                }
                else
                if (nextTileAssignment != this.ID)
                {
                    if (nextTileSurface == surface)
                        env.MergeAgents (nextTileAssignment, this);
                }
                return -1;
            }
        }

    }
}