using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;


namespace CoreMod
{
    public partial class ContinuousChunksModule : CreationNode
    {
        class AgentEnvironment
        {
            public bool Active { get; internal set; }
            int nextID;
            int[,] surface;
            public int[] Assigned;
            public List<ChunkAgent> Agents = new List<ChunkAgent> ();
            int sizeX;
            int sizeY;
            int tilesCount;
            EnvConnection envCon;
            public Direction[] TileConnections;
            public AgentEnvironment (int[,] tiles, TilesConnection tilesCon, EnvConnection envCon)
            {
                Active = true;
                nextID = 0;
                this.surface = tiles;
                this.envCon = envCon;
                sizeX = tiles.GetLength (0);
                sizeY = tiles.GetLength (1);
                tilesCount = sizeX * sizeY;
                Assigned = new int[tilesCount];
                for (int i = 0; i < tilesCount; i++)
                    Assigned [i] = -1;
        
                switch (tilesCon)
                {
                case TilesConnection.Four:
                    TileConnections = new Direction[] {
                Direction.Left,
                Direction.Top,
                Direction.Down,
                Direction.Right
                };
                    break;
                case TilesConnection.Eight:
                    TileConnections = new Direction[] {
                Direction.Left,
                Direction.Top,
                Direction.Down,
                Direction.Right,
                Direction.TopLeft,
                Direction.TopRight,
                Direction.DownLeft,
                Direction.DownRight
                };
                    break;
                }
            }
            #region agentsControl
            public void Update ()
            {
                bool activeAgents = false;
                for (int i = 0; i < Agents.Count; i++)
                    if (Agents [i].Active)
                    {
                        activeAgents = true;
                        Agents [i].Update ();
                    }
                if (!activeAgents)
                    Active = false;

            }
            public void NewAgent (int tile)
            {
                ChunkAgent agent = new ChunkAgent (nextID++, tile, this);
                Agents.Add (agent);
            }
    
            public void MergeAgents (int which, ChunkAgent intoWhat)
            {
                ChunkAgent mergedAgent = Agents.Find (x => x.ID == which);
                Agents.Remove (mergedAgent);
        
                intoWhat.Tiles.AddRange (mergedAgent.Tiles);
                intoWhat.Frontier.AddRange (mergedAgent.Frontier);
        
                for (int i = 0; i < mergedAgent.Tiles.Count; i++)
                    Assigned [mergedAgent.Tiles [i]] = intoWhat.ID;
                for (int i = 0; i < mergedAgent.Frontier.Count; i++)
                    Assigned [mergedAgent.Frontier [i]] = intoWhat.ID;
            }
            #endregion

            #region tilesControl
            public int Assignment (int tile)
            {
                return Assigned [tile];
            }
            
            public int Surface (int tile)
            {
                return surface [tile % sizeX, tile / sizeX];
            }
            
            public void AssignID (int tile, ChunkAgent agent)
            {
                Assigned [tile] = agent.ID;
            }
            public int GetNext (int tile, Direction dir)
            {
                int y = tile / sizeX;
                int x = tile % sizeX;
        
                switch (dir)
                {
            
                case Direction.Left:
                    x -= 1;
                    break;
                case Direction.Right:
                    x += 1;
                    break;
                case Direction.Top:
                    y -= 1;
                    break;
                case Direction.Down:
                    y += 1;
                    break;
                case Direction.TopLeft:
                    y -= 1;
                    x -= 1;
                    break;
                case Direction.TopRight:
                    y -= 1;
                    x += 1;
                    break;
                case Direction.DownLeft:
                    y += 1;
                    x -= 1;
                    break;
                case Direction.DownRight:
                    y += 1;
                    x += 1;
                    break;
                }
        
                switch (envCon)
                {
                case EnvConnection.Cylinder:
                    if (x < 0)
                        x = sizeX - 1;
                    else
                    if (x == sizeX)
                        x = 0;
            
                    if (y < 0)
                        y = 0;
                    else
                    if (y == sizeY)
                        y = sizeY - 1;
                    break;
                case EnvConnection.None:
                    if (x < 0)
                        x = 0;
                    else
                    if (x == sizeX)
                        x = sizeX - 1;
            
                    if (y < 0)
                        y = 0;
                    else
                    if (y == sizeY)
                        y = sizeY - 1;
                    break;
                case EnvConnection.Sphere:
            
                    int halfSize = sizeX / 2;
                    if (y < 0)
                    {
                        y = 0;
                        x += halfSize;
                    }
                    else
                    if (y == sizeY)
                    {
                        y = sizeY - 1;
                        x += halfSize;
                    }
            
            
                    if (x < 0)
                        x = sizeX - 1;
                    else
                    if (x >= sizeX)
                        x -= sizeX;
            
            
                    break;
                }
                return x + y * sizeX;
            }
            #endregion
        }
    }
}