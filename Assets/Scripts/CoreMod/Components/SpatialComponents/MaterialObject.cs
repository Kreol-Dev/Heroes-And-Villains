using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIO;

namespace CoreMod
{
	[AShared]
	[ECompName ("spatial_material")]
	public class MaterialObject : SpatialObject
	{
		[Defined ("form")]
		Form form;
		protected Dictionary<Form, Collider2D> colliders = new Dictionary<Form, Collider2D> ();

		protected override void Init ()
		{
			var body = gameObject.AddComponent<Rigidbody2D> ();
			body.isKinematic = true;
			gameObject.layer = PhysicsRoot.MaterialObjectsLayer;

		}

		protected override void PostDestroy ()
		{

		}

		public override EntityComponent CopyTo (GameObject go)
		{
			MaterialObject obj = base.CopyTo (go) as MaterialObject;
			obj.form = this.form;
			return obj;
		}

		public override void PostCreate ()
		{
			base.PostCreate ();
			Zone.AttachForm (form);
		}

		protected override void OnFormAdded (Form thisForm)
		{
			if (thisForm is CircleForm)
			{
				var circle = thisForm as CircleForm;
				CircleCollider2D circleCollider = gameObject.AddComponent<CircleCollider2D> ();
				circleCollider.offset = circle.Center;
				circleCollider.radius = circle.Radius;
				//circleCollider.transform.localScale = new Vector3 (circle.Radius, circle.Radius, circle.Radius);
				circleCollider.isTrigger = true;
				colliders.Add (thisForm, circleCollider);


			} else if (thisForm is RectForm)
			{
				var rect = thisForm as RectForm;
				BoxCollider2D rectCollider = gameObject.AddComponent<BoxCollider2D> ();
				rectCollider.offset = rect.Center;
				//rectCollider.transform.localScale = new Vector3 (rect.Size.x, 1, rect.Size.y);
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
				//circleCollider.transform.localScale = new Vector3 (circle.Radius, circle.Radius, circle.Radius);

			} else if (thisForm is RectForm)
			{
				var rect = thisForm as RectForm;
				BoxCollider2D rectCollider = colliders [thisForm] as BoxCollider2D;
				rectCollider.offset = rect.Center;
				//rectCollider.transform.localScale = new Vector3 (rect.Size.x, 1, rect.Size.y);
				rectCollider.size = rect.Size;
			}
		}


		void OnTriggerEnter (Collider other)
		{
			if (other.gameObject.layer == PhysicsRoot.MaterialObjectsLayer)
				OnObjectEntered (other.gameObject);
		}

		void OnTriggerExit (Collider other)
		{
			if (other.gameObject.layer == PhysicsRoot.MaterialObjectsLayer)
				OnObjectLeft (other.gameObject);
		}

	
	}

}