using UnityEngine;
using System.Collections;

namespace MapRoot
{
	public class HitsGetter
	{
		MapInteractor interactor;

		public HitsGetter (MapInteractor interactor)
		{
			this.interactor = interactor;
		}

		RaycastHit[] hits = new RaycastHit[10];
		ObjectHit[] objectHits = new ObjectHit[10];

		public ObjectHit[] ObjectHits { get { return objectHits; } }

		public int ObjectHitsCount { get { return objectHitsCount; } }

		int objectHitsCount = 0;

		public int GetHits (Vector2 screenPoint, out ObjectHit[] providedHits)
		{
			objectHitsCount = 0;
			Ray ray = Camera.main.ScreenPointToRay (screenPoint);
			int collidersCount = Physics.RaycastNonAlloc (ray, hits, 30);
			if (collidersCount >= hits.Length)
			{
				hits = new RaycastHit[collidersCount + 10];
				objectHits = new ObjectHit[collidersCount + 10];
				collidersCount = Physics.RaycastNonAlloc (ray, hits, 30);
			}
			for (int i = 0; i < collidersCount; i++)
			{
				LayerHandle[] handles = hits [i].transform.gameObject.GetComponents<LayerHandle> ();
				foreach (var handle in handles)
				{
					if (interactor.GetLayerState (handle.Layer) == InteractorState.Active)
					{
						objectHits [objectHitsCount++] = new ObjectHit (hits [i].transform, hits [i].point, interactor.GetInteractor (handle.Layer));
					}
				}
				
			}
			providedHits = objectHits;
			return objectHitsCount;
		}
	}


}

