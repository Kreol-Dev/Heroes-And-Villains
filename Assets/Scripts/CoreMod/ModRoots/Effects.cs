using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UIO;

namespace CoreMod
{
	public class Effect
	{
		[Defined ("effects")]
		List<EffectAspect> effects = new List<EffectAspect> ();

		public void Apply (GameObject go)
		{
			for (int i = 0; i < effects.Count; i++)
				effects [i].ApplyTo (go);
		}

		public void Reverse (GameObject go)
		{
			for (int i = 0; i < effects.Count; i++)
				effects [i].Reverse (go);
		}

	}

}


