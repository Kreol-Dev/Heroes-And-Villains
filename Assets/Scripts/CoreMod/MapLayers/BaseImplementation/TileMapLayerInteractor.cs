using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;

namespace CoreMod
{
    public abstract class TileMapLayerInteractor<TObject, TLayerObject, TLayer> : BaseMapLayerInteractor<TLayer>, 
    ITileMapInteractor<TObject, TLayerObject, TLayer> where TLayer : class, IMapLayer, ITileMapLayer<TLayerObject>
    {
        public event TileDelegate<TObject> TileClicked;

        public event TileDelegate<TObject> TileDeClicked;

        public event TileDelegate<TObject> TileRightClicked;

        public event TileDelegate<TObject> TileDeRightClicked;

        public event TileDelegate<TObject> TileHighlighted;

        public event TileDelegate<TObject> TileDeHighlighted;

        public event TileDelegate<TObject> TileHovered;

        public event TileDelegate<TObject> TileDeHovered;

        struct Tile
        {
            public TObject Content;
            public TileHandle Handle;
        }

        public abstract TObject ObjectFromLayerObject (TLayerObject obj);

        Tile FindTile (Vector3 point)
        {
            Tile tile = new Tile ();
            tile.Handle = point;
            tile.Content = ObjectFromLayerObject (tile.Handle.Content<TLayerObject> (Layer.Tiles));
            return tile;
        }

        protected override void OnHover (Transform obj, Vector3 point)
        {
            Tile tile = FindTile (point);
            if (TileHovered != null)
                TileHovered (tile.Handle, tile.Content);
        }

        protected override void OnEndHover (Transform obj, Vector3 point)
        {
            Tile tile = FindTile (point);
            if (TileDeHovered != null)
                TileDeHovered (tile.Handle, tile.Content);
        }

        protected override void OnClick (Transform obj, Vector3 point)
        {
            Tile tile = FindTile (point);
            if (TileClicked != null)
                TileClicked (tile.Handle, tile.Content);
        }

        protected override void OnEndClick (Transform obj, Vector3 point)
        {
            Tile tile = FindTile (point);
            if (TileDeClicked != null)
                TileDeClicked (tile.Handle, tile.Content);
        }

        protected override void OnHighlight (Transform obj, Vector3 point)
        {
            Tile tile = FindTile (point);
            if (TileHighlighted != null)
                TileHighlighted (tile.Handle, tile.Content);
        }

        protected override void OnEndHighlight (Transform obj, Vector3 point)
        {
            Tile tile = FindTile (point);
            if (TileDeHighlighted != null)
                TileDeHighlighted (tile.Handle, tile.Content);
        }

        protected override void Setup (ITable definesTable)
        {
            GameObject go = GameObject.Find ("MapCollider");
            LayerHandle handle = go.AddComponent<LayerHandle> ();
            handle.Layer = Find.Root<MapRoot.Map> ().GetLayer (Layer.Name);
            Transform transform = go.transform;
            transform.position = new Vector3 (Layer.Tiles.GetLength (0) / 2, Layer.Tiles.GetLength (1) / 2, 0);
            transform.localScale = new Vector3 (Layer.Tiles.GetLength (0), Layer.Tiles.GetLength (1), 0);

        }

    }
}

