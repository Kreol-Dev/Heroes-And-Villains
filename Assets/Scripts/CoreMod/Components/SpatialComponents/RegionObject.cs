using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoreMod
{
	public class RegionObject : SpatialObject
	{
		Dictionary<Form, Collider2D> colliders = new Dictionary<Form, Collider2D> ();

		protected override void Init ()
		{
			var body = gameObject.AddComponent<Rigidbody2D> ();
			body.isKinematic = true;
			gameObject.layer = PhysicsRoot.RegionObjectsLayer;
			gameObject.isStatic = true;
		}

		protected override void OnFormUpdated (Form thisForm)
		{
			BoxCollider2D boundsCollider = colliders [thisForm] as BoxCollider2D;
			float minX = float.MaxValue;
			float minY = float.MaxValue;
			float maxX = float.MinValue;
			float maxY = float.MinValue;
			foreach (var dot in (thisForm as DotsForm).GetDots())
			{
				if (dot.x > maxX)
					maxX = dot.x;
				else
					minX = dot.x;
				if (dot.y > maxY)
					maxY = dot.y;
				else
					minY = dot.y;
			}

			minX -= 1f;
			minY -= 1f;
			maxY += 1f;
			maxY += 1f;
			Rect rect = Rect.MinMaxRect (minX, minY, maxX, maxY);
			boundsCollider.size = rect.size;
			boundsCollider.offset = rect.center;
		}

		protected override void OnFormRemoved (Form thisForm)
		{
			Destroy (colliders [thisForm]);
			colliders.Remove (thisForm);
		}

		protected override void OnFormAdded (Form thisForm)
		{
			if (thisForm is DotsForm)
			{
				BoxCollider2D boundsCollider = gameObject.AddComponent<BoxCollider2D> ();
				float minX = float.MaxValue;
				float minY = float.MaxValue;
				float maxX = float.MinValue;
				float maxY = float.MinValue;
				foreach (var dot in (thisForm as DotsForm).GetDots())
				{
					if (dot.x > maxX)
						maxX = dot.x;
					else
						minX = dot.x;
					if (dot.y > maxY)
						maxY = dot.y;
					else
						minY = dot.y;
				}

				minX -= 1f;
				minY -= 1f;
				maxY += 1f;
				maxY += 1f;
				Rect rect = Rect.MinMaxRect (minX, minY, maxX, maxY);
				boundsCollider.size = rect.size;
				boundsCollider.offset = rect.center;
				colliders.Add (thisForm, boundsCollider);
			} else
			{
				Zone.DetachForm (thisForm);
				Scribe.LogFormatError ("{0} can't be added to {1} as it is a region object. Zone detached.", thisForm.GetType (), gameObject.name);

			}
		}

		void OnTriggerEnter (Collider other)
		{
		}

		void OnTriggerExit (Collider other)
		{
		}
	}


}

