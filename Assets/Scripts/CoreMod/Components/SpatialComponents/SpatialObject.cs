using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoreMod
{
	[DisallowMultipleComponent]
	public abstract class SpatialObject : EntityComponent
	{
		public override void LoadFromTable (UIO.ITable table)
		{
			Find.Root<ModsManager> ().Defs.LoadObjectAs<SpatialObject> (this, table);
		}

		public override EntityComponent CopyTo (GameObject go)
		{
			EntityComponent spatialObject = go.AddComponent (this.GetType ()) as EntityComponent;
			return spatialObject;
		}

		public override void PostCreate ()
		{
			
		}

		protected Scribe Scribe { get; private set; }

		public Zone Zone { get; internal set; }

		void Awake ()
		{
			var events = Find.Root<CoreModEvents> ();
			events.SpatialObjectCreated.Dispatch (this);
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


