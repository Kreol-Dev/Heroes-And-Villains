using UnityEngine;
using System.Collections;
using Demiurg;
namespace CoreMod
{
    public class PointsVisualizer : CreationNode
    {
        NodeInput<Texture2D> baseTextureI;
        NodeInput<TileRef[]> tiles;
        FloatParam red;
        FloatParam green;
        FloatParam blue;
        protected override void SetupIOP ()
        {
            baseTextureI = Input<Texture2D> ("base_texture");
            tiles = Input<TileRef[]> ("tiles");
            red = Config<FloatParam> ("tile_red");
            green = Config<FloatParam> ("tile_green");
            blue = Config<FloatParam> ("tile_blue");
        }
        protected override void Work ()
        {
            Color tileColor = new Color (red, green, blue);
            Texture2D texture = Object.Instantiate<Texture2D> (baseTextureI.Content);
            for (int i = 0; i < tiles.Content.Length; i++)
                texture.SetPixel (tiles.Content [i].X, tiles.Content [i].Y, tileColor);
            texture.Apply ();
            GameObject visGO = new GameObject (this.GetType ().ToString () + ": " + Name);
            visGO.AddComponent<SpriteRenderer> ().sprite = 
                Sprite.Create (
                    texture, 
                    Rect.MinMaxRect (0, 0, texture.width, texture.height),
                    Vector2.zero);

        }
    }
    
}