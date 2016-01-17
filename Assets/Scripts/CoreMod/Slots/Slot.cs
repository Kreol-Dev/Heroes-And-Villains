using UnityEngine;
using System.Collections;
using CoreMod;
using Demiurg.Core.Extensions;
using System.Collections.Generic;
using System;

namespace CoreMod
{
	public class Slot : SlotComponent
	{
		public TagsCollection Tags;
		public Dictionary<object, object> CustomData;
		public GameObject Replacer;

		void Awake ()
		{
			CustomData = new Dictionary<object, object> ();
			Tags = new TagsCollection ();
		}
	}


}


