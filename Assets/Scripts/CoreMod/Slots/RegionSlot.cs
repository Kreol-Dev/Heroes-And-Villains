using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIO;

namespace CoreMod
{
	[ASlotComponent ("Region")]
	[AShared]
	public class RegionSlot : SlotComponent
	{
		[Defined ("size")]
		public int Size { get { return Tiles.Count; } }

		[Defined ("is_region")]
		public bool IsRegion = false;
		public List<TileHandle> Tiles;
		public string TargetLayerName;

		public override void FillComponent (GameObject go)
		{
			//var tiles = go.AddComponent<TilesComponent> ();
			//tiles.Receive (this);
			var regionObject = go.GetComponent<RegionObject> ();
			if (regionObject == null)
			{
				Scribes.Find ("SLOT COMPONENT").LogFormatWarning ("Couldn't fill the component of an object {0} with regionSlot data {1}", 
				                                                  go.name, this.gameObject.name);
				return;
			}
			Vector2[] dots = new Vector2[Tiles.Count];
			for (int i = 0; i < dots.Length; i++)
				dots [i] = Tiles [i].Center;
			regionObject.Zone.AttachForm (new DotsForm (dots));

		}
	}

}

