using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MapRoot
{
	public class HitsGetter
	{

		public HitsGetter (IEnumerable<IMapLayerInteractor> interactors)
		{
			foreach (var interactor in interactors)
				allegianceDict.Add (interactor, new HashSet<Transform> ());
		}

		RaycastHit2D[] hits = new RaycastHit2D[10];
		ObjectHit[] realmHits = new ObjectHit[10];
		Dictionary<IMapLayerInteractor, HashSet<Transform>> allegianceDict = new Dictionary<IMapLayerInteractor, HashSet<Transform>> ();

		public ObjectHit[] RealmHits { get { return realmHits; } }

		public Dictionary<IMapLayerInteractor, HashSet<Transform>> AllegianceHits { get { return allegianceDict; } }

		public int ObjectHitsCount { get { return realmHitsCount; } }


		int realmHitsCount = 0;

		public void GetHits (Vector2 screenPoint)
		{
			foreach (var allegianceSet in allegianceDict)
				allegianceSet.Value.Clear ();
			
			realmHitsCount = 0;
			var point = Camera.main.ScreenToWorldPoint (screenPoint);
			int collidersCount = Physics2D.RaycastNonAlloc (point, Vector2.zero, hits); //(point, hits, 30);
			if (collidersCount >= hits.Length)
			{
				hits = new RaycastHit2D[collidersCount + 2];
				realmHits = new ObjectHit[collidersCount + 2];
				collidersCount = Physics2D.RaycastNonAlloc (point, Vector2.zero, hits);
			}
			for (int i = 0; i < collidersCount; i++)
			{
				InteractorRealm[] realms = hits [i].transform.gameObject.GetComponents<InteractorRealm> ();
				if (realms.Length != 0)
				{
					foreach (var realm in realms)
					{
						realmHits [realmHitsCount++] = new ObjectHit (hits [i].transform, hits [i].point, realm.Interactor);
					}
				} else
				{
					InteractorAllegiance allegiance = hits [i].transform.gameObject.GetComponent<InteractorAllegiance> ();
					if (allegiance == null)
						continue;
					HashSet<Transform> transforms = allegianceDict [allegiance.Interactor];
					transforms.Add (hits [i].transform);
				}
				
			}
		}
	}


}

