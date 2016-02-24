using UnityEngine;
using System.Collections;
using CoreMod;
using UIO;
using System.Collections.Generic;
using System;

namespace CoreMod
{
	public class Slot : SlotComponent
	{
		public int Similarity;
		public TagsCollection Tags;
		public Dictionary<object, object> CustomData;
		public GameObject Replacer;

		void Awake ()
		{
			
			CustomData = new Dictionary<object, object> ();
			Tags = new TagsCollection ();
			gameObject.AddComponent<TagsVisual> ().Setup (Tags);
		}
	}


}


