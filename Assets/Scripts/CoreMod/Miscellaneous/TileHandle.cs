using UnityEngine;
using System.Collections;

namespace CoreMod
{
    public struct TileHandle
    {
        public int X { get; internal set; }

        public int Y { get; internal set; }

        public TileHandle (int x, int y)
        {
            X = x; 
            Y = y;
        }

        public T Content<T> (T[,] layer)
        {
            return layer [X, Y];
        }

        public static bool operator == (TileHandle A, TileHandle B)
        {
            return A.X == B.X && A.Y == B.Y;
        }

        public static bool operator != (TileHandle A, TileHandle B)
        {
            return A.X != B.X || A.Y != B.Y;
        }

        public override bool Equals (object obj)
        {
            TileHandle handle = (TileHandle)obj;

            return X == handle.X && Y == handle.Y;

        }

        public override int GetHashCode ()
        {
            unchecked
            {         
                int hash = 27;
                hash = (13 * hash) + X.GetHashCode ();
                hash = (13 * hash) + Y.GetHashCode ();
                return hash;
            }
        }

        public static implicit operator TileHandle (Vector2 vec)
        {
            return new TileHandle ((int)vec.x, (int)vec.y);
        }

        public static implicit operator TileHandle (Vector3 vec)
        {
            return new TileHandle ((int)vec.x, (int)vec.y);
        }
    }
}

