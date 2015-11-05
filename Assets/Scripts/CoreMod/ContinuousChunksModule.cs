
using System.Collections;
using System.Collections.Generic;
using Demiurg;
namespace CoreMod
{
    public class ContinuousChunksModule : CreationNode
    {
        class Chunk
        {
            public IntParam Value = new IntParam ("value");
        }

        NodeInput<int[,]> mainI;
        NodeOutput<List<List<int>>> mainO;

        StringParam planetConnectivity;
        ArrayParam<Chunk> chunks;
        protected override void SetupIOP ()
        {
            mainI = Input<int[,]> ("main");
            mainO = Output<List<List<int>>> ("main");
            planetConnectivity = Config<StringParam> ("planet_connectivity");
            chunks = Config<ArrayParam<Chunk>> ("chunks");
        }
        protected override void Work ()
        {
			
        }
    }
    #region enums
    public enum TilesConnection
    {
        Four,
        Eight
    }
    public enum EnvConnection
    {
        None,
        Cylinder,
        Sphere
    }
    public enum Direction
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
   
    class AgentEnvironment
    {
        int[,] surfaces;
        int[] assigned;
        int sizeX;
        int sizeY;
        int tilesCount;
        TilesConnection tilesCon;
        EnvConnection envCon;
        public AgentEnvironment (int[,] tiles, TilesConnection tilesCon, EnvConnection envCon)
        {
            this.surfaces = tiles;
            this.tilesCon = tilesCon;
            this.envCon = envCon;
            sizeX = tiles.GetLength (0);
            sizeY = tiles.GetLength (1);
            tilesCount = sizeX * sizeY;
            assigned = new int[tilesCount];
            for (int i = 0; i < tilesCount; i++)
                assigned [i] = 0;
        }
        public int Tile (int x, int y)
        {
            return x + y * sizeX;
        }
        /*public void AssignToAgent (ChunkAgent agent, int tile)
        {
            agent.Frontier.Add (tile);

        }*/

        public int GetNext (int tile, Direction dir)
        {
            int y = tile / sizeX;
            int x = tilesCount - y;

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
                if (x = sizeX)
                    x = 0;

                if (y < 0)
                    y = 0;
                else
                if (y = sizeY)
                    y = sizeY - 1;
                break;
            case EnvConnection.None:
                if (x < 0)
                    x = 0;
                else
                if (x = sizeX)
                    x = sizeX - 1;

                if (y < 0)
                    y = 0;
                else
                if (y = sizeY)
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
                if (y = sizeY)
                {
                    y = sizeY - 1;
                    x += halfSize;
                }
                    

                if (x < 0)
                    x = sizeX - 1;
                else
                if (x >= sizeX)
                    x = 0;


                break;
            }
            return x + y * sizeX;
        }
    }
    class ChunkAgent
    {
        public List<int> Tiles = new List<int> ();
        public List<int> Frontier = new List<int> ();
        public ChunkAgent (int tile, int value, int surface, AgentEnvironment env)
        {


        }
    }

}





