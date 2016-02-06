using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;
using System;

namespace CoreMod
{
	public abstract class AbstractSpriteMapRenderer<TLayerObject, TLayer, TCollection> : BaseMapLayerRenderer<TLayer, TCollection>
		where TLayer: MapLayer<TCollection> where TCollection : class, IMapCollection
	{
		public override void ChangeState (RepresenterState state)
		{
			switch (state)
			{
			case RepresenterState.Active:
				RegisterCallbacks (UpdateAll, TileUpdated);
				break;
			case RepresenterState.NotActive:
				UnregisterCallbacks (UpdateAll, TileUpdated);
				break;
			}
		}

		protected abstract Sprite GetSprite (TLayerObject obj);

		protected abstract TLayerObject GetLayerObject (int x, int y);

		protected abstract int SizeX { get; }

		protected abstract int SizeY { get; }

		protected abstract void RegisterCallbacks (Action updateAll, Action<TileHandle> updateTile);

		protected abstract void UnregisterCallbacks (Action updateAll, Action<TileHandle> updateTile);


		void TileUpdated (TileHandle tile)
		{

			SetTile (tile.X, tile.Y, GetSprite (GetLayerObject (tile.X, tile.Y)));
		}



		void UpdateAll ()
		{
			for (int i = 0; i < SizeX; i++)
				for (int j = 0; j < SizeY; j++)
					SetTile (i, j, GetSprite (GetLayerObject (i, j)));
		}

		int chunksSizeX;
		int chunksSizeY;
		SpriteMapChunk[,] chunks;

		protected override void Setup (ITable definesTable)
		{
			chunksSizeX = definesTable.GetInt ("SPRITE_CHUNK_SIZE_X");
			chunksSizeY = definesTable.GetInt ("SPRITE_CHUNK_SIZE_Y");
			int chunkCountX = SizeX / chunksSizeX;
			int chunkCountY = SizeY / chunksSizeY;
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

