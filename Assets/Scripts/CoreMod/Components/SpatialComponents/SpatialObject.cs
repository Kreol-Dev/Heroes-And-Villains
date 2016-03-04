using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoreMod
{
	[DisallowMultipleComponent]
	public abstract class SpatialObject : MonoBehaviour
	{

		protected Scribe Scribe { get; private set; }

		public Zone Zone { get; internal set; }

		void Awake ()
		{
			Scribe = Scribes.Find ("SPATIAL OBJECT");
			Zone = new Zone ();
			Zone.FormAdded += OnFormAdded; 
			Zone.FormRemoved += OnFormRemoved;
			Zone.FormUpdated += OnFormUpdated;
			Init ();
		}

		protected abstract void Init ();

		protected abstract void OnFormUpdated (Form thisForm);

		protected abstract void OnFormRemoved (Form thisForm);

		protected abstract void OnFormAdded (Form thisForm);

		public delegate void GODelegate (GameObject go);

		public event GODelegate ObjectEntered;
		public event GODelegate ObjectLeft;

		protected void OnObjectEntered (GameObject go)
		{
			if (ObjectEntered != null)
				ObjectEntered (go);
		}

		protected void OnObjectLeft (GameObject go)
		{
			if (ObjectLeft != null)
				ObjectLeft (go);
		}


	}

}


