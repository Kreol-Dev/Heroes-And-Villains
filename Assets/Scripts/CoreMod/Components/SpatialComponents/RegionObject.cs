using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIO;

namespace CoreMod
{
	[AShared]
	[ECompName ("spatial_region")]
	public class RegionObject : SpatialObject
	{
		static int regionsCount = 0;
		static int objectsCap = 10;
		int updateOffset;
		Dictionary<Form, Collider2D> colliders = new Dictionary<Form, Collider2D> ();
		HashSet<TileHandle> tiles = new HashSet<TileHandle> ();

		public IEnumerable<TileHandle> Tiles { get { return tiles; } }

		MapHandle map;

		protected override void PostDestroy ()
		{

		}

		protected override void Init ()
		{ 
			regionsCount++;
			updateOffset = regionsCount % objectsCap;
			StartCoroutine (InTilesCoroutine ());
			map = Find.Root<TilesRoot> ().MapHandle;
			var body = gameObject.AddComponent<Rigidbody2D> ();
			body.isKinematic = true;
			gameObject.layer = PhysicsRoot.RegionObjectsLayer;
			gameObject.isStatic = true;
		}

		protected override void OnFormUpdated (Form thisForm)
		{
			BoxCollider2D boundsCollider = colliders [thisForm] as BoxCollider2D;
			Rect rect = FindRect ((thisForm as DotsForm).GetDots ());
			boundsCollider.size = rect.size;
			boundsCollider.offset = rect.center;
			ProjectZoneOnTiles ();
		}

		protected override void OnFormRemoved (Form thisForm)
		{
			Destroy (colliders [thisForm]);
			colliders.Remove (thisForm);
			ProjectZoneOnTiles ();
		}

		Rect FindRect (IEnumerable<Vector2> dots)
		{
			float minX = float.MaxValue;
			float minY = float.MaxValue;
			float maxX = float.MinValue;
			float maxY = float.MinValue;
			foreach (var dot in dots)
			{
				if (dot.x > maxX)
					maxX = dot.x;
				if (dot.x < minX)
					minX = dot.x;
				if (dot.y > maxY)
					maxY = dot.y;
				if (dot.y < minY)
					minY = dot.y;
			}
			minX -= 0.55f;
			minY -= 0.55f;
			maxX += 0.55f;
			maxY += 0.55f;
			return Rect.MinMaxRect (minX, minY, maxX, maxY);
		}

		protected override void OnFormAdded (Form thisForm)
		{
			if (thisForm is DotsForm)
			{
				BoxCollider2D boundsCollider = gameObject.AddComponent<BoxCollider2D> ();
				boundsCollider.isTrigger = true;
				Rect rect = FindRect ((thisForm as DotsForm).GetDots ());
				boundsCollider.size = rect.size;
				boundsCollider.offset = rect.center;
				colliders.Add (thisForm, boundsCollider);
				ProjectZoneOnTiles ();
			} else
			{
				Zone.DetachForm (thisForm);
				Scribe.LogFormatError ("{0} can't be added to {1} as it is a region object. Zone detached.", thisForm.GetType (), gameObject.name);

			}
		}

		void OnTriggerEnter (Collider other)
		{
			if (tiles.Contains (map.GetHandle (other.transform.position)))
			{
				inRegion.Add (other.transform);
				OnObjectEntered (other.gameObject);
			} else
				inBounds.Add (other.transform);
		}

		HashSet<Transform> inBounds = new HashSet<Transform> ();
		HashSet<Transform> inRegion = new HashSet<Transform> ();

		void OnTriggerExit (Collider other)
		{
//			this.OnObjectLeft (other.gameObject);
			if (inBounds.Remove (other.transform))
				return;
			if (inRegion.Remove (other.transform))
				OnObjectLeft (other.gameObject);
		}

		List<Transform> toRegion = new List<Transform> ();
		List<Transform> toBounds = new List<Transform> ();


		public bool ConfirmExistanceIn (Vector2 point)
		{
			return tiles.Contains (map.GetHandle (point));
		}

		IEnumerator InTilesCoroutine ()
		{
			for (int i = 0; i < updateOffset; i++)
				yield return null;
			while (true)
			{
				if (this.isActiveAndEnabled)
				{
					toRegion.Clear ();
					toBounds.Clear ();
					foreach (var bound in inBounds)
						if (tiles.Contains (map.GetHandle (bound.position)))
							toRegion.Add (bound);
					foreach (var bound in inRegion)
						if (!tiles.Contains (map.GetHandle (bound.position)))
							toBounds.Add (bound);
					for (int i = 0; i < toRegion.Count; i++)
					{
						inBounds.Remove (toRegion [i]);
						inRegion.Add (toRegion [i]);
						OnObjectEntered (toRegion [i].gameObject);
					}

					for (int i = 0; i < toBounds.Count; i++)
					{
						inRegion.Remove (toRegion [i]);
						inBounds.Add (toRegion [i]);
						OnObjectLeft (toRegion [i].gameObject);
					}
				}
				for (int i = 0; i < objectsCap; i++)
					yield return null;
			}
		}


		void ProjectZoneOnTiles ()
		{
			tiles.Clear ();
			foreach (var formPair in colliders)
			{
				
				/*if (formPair.Key is CircleForm)
				{
					var circleForm = formPair.Key as CircleForm;
				} else if (formPair.Key is RectForm)
				{
					var rectForm = formPair.Key as RectForm;
				} else if (formPair.Key is PolygonForm)
				{
					var polygonForm = formPair.Key as PolygonForm;
				} else*/

				if (formPair.Key is DotsForm)
				{
					var dotsForm = formPair.Key as DotsForm;
					foreach (var dot in dotsForm.GetDots())
						tiles.Add (map.GetHandle (dot));

				} 
			}
		}



	}
}

