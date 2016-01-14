using UnityEngine;
using System.Collections;
using Demiurg;
using Demiurg.Core;


namespace CoreMod
{
    public class PointsVisualizer :Demiurg.Core.Avatar
    {
        [AInput ("texture")]
        Texture2D baseTextureI;
        [AInput ("tiles")]
        TileRef[] tiles;
        [AConfig ("tile_color")]
        Color tileColor;

        public override void Work ()
        {
            Texture2D texture = Object.Instantiate<Texture2D> (baseTextureI);
            for (int i = 0; i < tiles.Length; i++)
                texture.SetPixel (tiles [i].X, tiles [i].Y, tileColor);
            texture.Apply ();
            GameObject go = new GameObject (Name);
            Map map = go.AddComponent<Map> ();
            map.Name = this.Name;
            map.Sprite = Sprite.Create (
                texture, 
                Rect.MinMaxRect (0, 0, texture.width, texture.height),
                Vector2.zero, 1f);
            map.Setup ();
            FinishWork ();

        }
    }
    
}