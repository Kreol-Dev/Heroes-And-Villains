using UnityEngine;
using System.Collections;

namespace MapRoot
{
	
	public struct ObjectHit
	{
		public IMapCollectionInteractor Interactor { get; internal set; }

		public Transform Transform { get; internal set; }

		public Vector3 Position { get; internal set; }

		public ObjectHit (Transform transform, Vector3 position, IMapCollectionInteractor interactor)
		{
			Transform = transform;
			Position = position;
			Interactor = interactor;
		}
	}

}