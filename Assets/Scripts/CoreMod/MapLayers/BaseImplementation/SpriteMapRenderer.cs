using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;

namespace CoreMod
{
	public abstract class SpriteMapRenderer<TObject, TLayerObject, TLayer, TInteractor> : BaseMapLayerRenderer<TLayer, TInteractor>
		where TLayer: class, IMapLayer, ITileMapLayer<TLayerObject> where TInteractor : TileMapLayerInteractor<TLayer>
	{
		public override void ChangeState (RepresenterState state)
		{
			switch (state)
			{
			case RepresenterState.Active:
				Interactor.TileSelected += Clicked;
				Interactor.TileDeselected += DeClicked;
				Interactor.TileHovered += Hovered;
				Interactor.TileDeHovered += DeHovered;
				Layer.MassUpdate.AddListener (UpdateAll);
				Layer.TileUpdated.AddListener (TileUpdated);
				break;
			case RepresenterState.NotActive:
				Interactor.TileSelected -= Clicked;
				Interactor.TileDeselected -= DeClicked;
				Interactor.TileHovered -= Hovered;
				Interactor.TileDeHovered -= DeHovered;
				Layer.MassUpdate.RemoveListener (UpdateAll);
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

		void TileUpdated (TileHandle tile, TLayerObject obj)
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

		protected override void Setup (ITable definesTable)
		{
			mapWidth = Layer.Tiles.GetLength (0);
			mapHeight = Layer.Tiles.GetLength (1);
			chunksSizeX = definesTable.GetInt ("SPRITE_CHUNK_SIZE_X");
			chunksSizeY = definesTable.GetInt ("SPRITE_CHUNK_SIZE_Y");
			int chunkCountX = mapWidth / chunksSizeX;
			int chunkCountY = mapHeight / chunksSizeY;
			chunks = new SpriteMapChunk[chunkCountX, chunkCountX];
			for (int i = 0; i < chunkCountX; i++)
				for (int j = 0; j < chunkCountX; j++)
				{
					GameObject chunkGO = new GameObject (string.Format ("ChunkGO:  {0}:{1}", i, j));
					chunkGO.transform.position = new Vector3 (chunksSizeX * i, chunksSizeY * j);
					SpriteMapChunk chunk = chunkGO.AddComponent<SpriteMapChunk> ();
					chunk.Setup (chunksSizeX, chunksSizeY);
					chunks [i, j] = chunk;

				}
			UpdateAll ();
		}

		void SetTile (int x, int y, Sprite sprite)
		{
			int chunkX = x / chunksSizeX;
			int chunkY = y / chunksSizeY;

			SpriteMapChunk chunk = chunks [chunkX, chunkY];
			int chunkLocalX = x % chunksSizeX;
			int chunkLocalY = y % chunksSizeY;
			chunk.SetTileSprite (chunkLocalX, chunkLocalY, sprite);

		}



	}

}

