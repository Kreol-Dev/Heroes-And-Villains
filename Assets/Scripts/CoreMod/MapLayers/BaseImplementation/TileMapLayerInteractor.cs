using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;
using System.Collections.Generic;
using System.Text;

namespace CoreMod
{
	public class TileMapLayerInteractor : BaseMapLayerInteractor<TileMapLayer>, IObjectsInteractor<TileHandle>
	{

		HashSet<TileHandle> selectedTiles = new HashSet<TileHandle> ();

		public event ObjectDelegate<TileHandle> ObjectSelected;

		public event ObjectDelegate<TileHandle> ObjectDeSelected;

		TileHandle hoveredTile = null;

		public event ObjectDelegate<TileHandle> ObjectHovered;

		public event ObjectDelegate<TileHandle> ObjectDeHovered;


		public override void OnHover (Vector2 point, HashSet<Transform> encounters, ref HashSet<object> hoveredObjects)
		{
			TileHandle handle = Layer.MapHandle.GetHandle (point);
			if (handle != null)
			{
				if (hoveredTile != handle)
				{
					if (hoveredTile != null)
						ObjectDeHovered (hoveredTile);
					hoveredTile = handle;
					ObjectHovered (hoveredTile);
				}					
				hoveredObjects.Add (handle);
			} else
			{
				if (hoveredTile != null)
				{
					ObjectDeHovered (hoveredTile);
					hoveredTile = null;
				}
			}
		}

		public override object OnSelect (Vector2 point, HashSet<object> selectables)
		{
			TileHandle handle = Layer.MapHandle.GetHandle (point);
			if (handle == null)
				return null;
			else if (!selectables.Contains (handle))
			{
				return null;
			} else if (selectedTiles.Add (handle))
			{
				ObjectSelected (handle);
				return handle;
			} 		
			return null;
		}

		public override object OnDeSelect (Vector2 point)
		{
			TileHandle handle = Layer.MapHandle.GetHandle (point);
			if (selectedTiles.Remove (handle))
			{
				ObjectDeSelected (handle);
				return handle;
			}
			return null;
		}

		HashSet<TileHandle> cachedSet = new HashSet<TileHandle> ();

		public override IEnumerable<object> OnDeselectAll ()
		{
			cachedSet.Clear ();
			HashSet<TileHandle> tempSet = selectedTiles;
			selectedTiles = cachedSet;
			cachedSet = tempSet;
			foreach (var tile in cachedSet)
				ObjectDeSelected (tile);
			return cachedSet as IEnumerable<object>;
		}

		public override IEnumerable<object> OnMassSelect (Vector2 minCorner, Vector2 maxCorner)
		{
			return null;
		}


		protected override void Setup (ITable definesTable)
		{
//			ObjectSelected += (obj) => Debug.LogFormat ("TILE SElECTED: {0} {1}", obj.X, obj.Y);
//
//			ObjectDeSelected += (obj) => Debug.LogFormat ("TILE DESELECTED: {0} {1}", obj.X, obj.Y);
//
//			ObjectHovered += (obj) => Debug.LogFormat ("TILE HOVERED: {0} {1}", obj.X, obj.Y);
//
//			ObjectDeHovered += (obj) => Debug.LogFormat ("TILE DEHOVERED: {0} {1}", obj.X, obj.Y);

			ObjectSelected += (obj) =>{};
	
			ObjectDeSelected += (obj) => {};
	
			ObjectHovered += (obj) =>{};
	
			ObjectDeHovered += (obj) => {};
			GameObject go = GameObject.Find ("MapCollider");
			InteractorRealm handle = go.AddComponent<InteractorRealm> ();
			handle.Interactor = this;
			Transform transform = go.transform;
			transform.position = new Vector3 (Layer.MapHandle.SizeX / 2, Layer.MapHandle.SizeY / 2, 0);
			transform.localScale = new Vector3 (Layer.MapHandle.SizeX, Layer.MapHandle.SizeY, 0);

		}

	}
}

