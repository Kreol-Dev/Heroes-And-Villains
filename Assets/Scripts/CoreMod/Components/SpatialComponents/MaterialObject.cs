using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoreMod
{
	public class MaterialObject : SpatialObject
	{
		protected Dictionary<Form, Collider2D> colliders = new Dictionary<Form, Collider2D> ();

		protected override void Init ()
		{
			var body = gameObject.AddComponent<Rigidbody2D> ();
			body.isKinematic = true;
			gameObject.layer = PhysicsRoot.MaterialObjectsLayer;
		}

		protected override void OnFormAdded (Form thisForm)
		{
			if (thisForm is CircleForm)
			{
				var circle = thisForm as CircleForm;
				CircleCollider2D circleCollider = gameObject.AddComponent<CircleCollider2D> ();
				circleCollider.offset = circle.Center;
				circleCollider.radius = circle.Radius;
				circleCollider.isTrigger = true;
				colliders.Add (thisForm, circleCollider);


			} else if (thisForm is RectForm)
			{
				var rect = thisForm as RectForm;
				BoxCollider2D rectCollider = gameObject.AddComponent<BoxCollider2D> ();
				rectCollider.offset = rect.Center;
				rectCollider.size = rect.Size;
				rectCollider.isTrigger = true;
				colliders.Add (rect, rectCollider);
			} else
			{
				Zone.DetachForm (thisForm);
				Scribe.LogFormatError ("{0} can't be added to {1} as it is a material object. Zone detached.", thisForm.GetType (), gameObject.name);
			}
		}

		protected override void OnFormRemoved (Form thisForm)
		{
			Destroy (colliders [thisForm]);
			colliders.Remove (thisForm);
		}

		protected override void OnFormUpdated (Form thisForm)
		{
			if (thisForm is CircleForm)
			{
				var circle = thisForm as CircleForm;
				CircleCollider2D circleCollider = colliders [thisForm] as CircleCollider2D;
				circleCollider.offset = circle.Center;
				circleCollider.radius = circle.Radius;

			} else if (thisForm is RectForm)
			{
				var rect = thisForm as RectForm;
				BoxCollider2D rectCollider = colliders [thisForm] as BoxCollider2D;
				rectCollider.offset = rect.Center;
				rectCollider.size = rect.Size;
			}
		}


		void OnTriggerEnter (Collider other)
		{
			//if (other.gameObject.GetComponent<MaterialObject> () != null)
			OnObjectEntered (other.gameObject);
		}

		void OnTriggerExit (Collider other)
		{
			//if (other.gameObject.GetComponent<MaterialObject> () != null)
			OnObjectLeft (other.gameObject);
		}

	
	}

}