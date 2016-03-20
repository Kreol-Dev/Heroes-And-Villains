using UnityEngine;
using System.Collections;
using UIO;

namespace CoreMod
{
	[AShared]
	[ECompName ("entity")]
	public class Entity : EntityComponent
	{
		// entitiesLayer;
		IListMapLayer<GameObject> gosLayer;

		public override void LoadFromTable (ITable table)
		{
			//Find.Root<ModsManager> ().Defs.LoadObject<Entity> (this, table);
			string layerName = table.GetString ("layer_name");
			gosLayer = Find.Root<MapRoot.Map> ().GetLayer (layerName) as IListMapLayer<GameObject>;
		}

		public override EntityComponent CopyTo (GameObject go)
		{
			Entity entity = go.AddComponent<Entity> ();
			entity.gosLayer = this.gosLayer;
			return entity;
		}

		public override void PostCreate ()
		{

			gosLayer.AddObject (this.gameObject);
		}

		protected override void PostDestroy ()
		{

		}
	}

}

