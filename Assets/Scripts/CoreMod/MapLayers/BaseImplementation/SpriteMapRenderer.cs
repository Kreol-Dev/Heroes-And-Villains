using UnityEngine;
using System.Collections;
using MapRoot;
using System.Collections.Generic;
using UIO;

namespace CoreMod
{
	public abstract class SpriteMapRenderer<TLayerObject, TLayer> : BaseMapLayerRenderer<TLayer, TileMapLayerInteractor>
		where TLayer: class, IMapLayer, ITileMapLayer<TLayerObject>
	{
		public override void ChangeState (RepresenterState state)
		{
			switch (state)
			{
			case RepresenterState.Active:
				Interactor.ObjectSelected += Clicked;
				Interactor.ObjectDeSelected += DeClicked;
				Interactor.ObjectHovered += Hovered;
				Interactor.ObjectDeHovered += DeHovered;
				Layer.TileUpdated.AddListener (TileUpdated);
				break;
			case RepresenterState.NotActive:
				Interactor.ObjectSelected -= Clicked;
				Interactor.ObjectDeSelected -= DeClicked;
				Interactor.ObjectHovered -= Hovered;
				Interactor.ObjectDeHovered -= DeHovered;
				Layer.TileUpdated.RemoveListener (TileUpdated);
				break;
			}
		}

		protected abstract Sprite GetSprite (TLayerObject obj);

		void Clicked (TileHandle tile)
		{
		}

		void DeClicked (TileHandle tile)
		{
		}

		void Hovered (TileHandle tile)
		{
		}

		void DeHovered (TileHandle tile)
		{
		}

		void TileUpdated (TileHandle tile)
		{

			SetTile (tile.X, tile.Y, GetSprite (tile.Get<TLayerObject> (Layer.Tiles)));
		}

		void UpdateAll ()
		{
			for (int i = 0; i < mapWidth; i++)
				for (int j = 0; j < mapHeight; j++)
					SetTile (i, j, GetSprite (Layer.Tiles [i, j]));
		}

		int mapWidth;
		int mapHeight;
		int chunksSizeX;
		int chunksSizeY;
		SpriteMapChunk[,] chunks;
		protected Dictionary<string, GraphicsTile> tiles = new Dictionary<string, GraphicsTile> ();

		protected override void Setup (ITable definesTable, ITable rendererData)
		{
			int rendererPriority = rendererData.GetInt ("priority");
			string atlasName = rendererData.GetString ("atlas");
			ITable tilesTable = rendererData.GetTable ("tiles", null);
			Sprites sprites = Find.Root<Sprites> ();
			if (tilesTable != null)
			{
				foreach (var key in tilesTable.GetKeys())
				{
					ITable graphicsTable = tilesTable.GetTable (key);
					string spriteName = graphicsTable.GetString ("sprite");
					int priority = graphicsTable.GetInt ("priority");
					var sprite = sprites.GetSprite (atlasName, spriteName);
					GraphicsTile tile = new GraphicsTile (sprite, priority, key as string);
					tiles.Add (key as string, tile);
				}
			}
			ITable rulesTable = rendererData.GetTable ("rules");
			ReadRules (rulesTable);


			mapWidth = Layer.Tiles.GetLength (0);
			mapHeight = Layer.Tiles.GetLength (1);
			chunksSizeX = definesTable.GetInt ("SPRITE_CHUNK_SIZE_X");
			chunksSizeY = definesTable.GetInt ("SPRITE_CHUNK_SIZE_Y");
			int chunkCountX = mapWidth / chunksSizeX;
			int chunkCountY = mapHeight / chunksSizeY;
			chunks = new SpriteMapChunk[chunkCountX, chunkCountX];

			Material sharedMaterial = GameObject.Instantiate (Resources.Load<Material> ("DefaultSpriteMaterial")) as Material;
			for (int i = 0; i < chunkCountX; i++)
				for (int j = 0; j < chunkCountX; j++)
				{
					GameObject chunkGO = new GameObject (string.Format ("ChunkGO:  {0}:{1}", i, j));
					chunkGO.transform.position = new Vector3 (chunksSizeX * i, chunksSizeY * j);
					SpriteMapChunk chunk = chunkGO.AddComponent<SpriteMapChunk> ();
					chunk.Setup (chunksSizeX, chunksSizeY, sharedMaterial, rendererPriority);
					chunks [i, j] = chunk;

				}
			UpdateAll ();
		}

		protected abstract void ReadRules (ITable rulesTable);

		void SetTile (int x, int y, Sprite sprite)
		{
			if (sprite == null)
				return;
			int chunkX = x / chunksSizeX;
			int chunkY = y / chunksSizeY;

			SpriteMapChunk chunk = chunks [chunkX, chunkY];
			int chunkLocalX = x % chunksSizeX;
			int chunkLocalY = y % chunksSizeY;
			chunk.SetTileSprite (chunkLocalX, chunkLocalY, sprite);

		}



	}

}

