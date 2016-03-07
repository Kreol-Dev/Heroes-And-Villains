using UnityEngine;
using System.Collections;
using UIO;
using System.Collections.Generic;

namespace CoreMod
{
	public class BiomeSharedData
	{
		public string BiomeType { get; internal set; }

		public BiomeSharedData (string biomeType)
		{
			BiomeType = biomeType;
		}
	}


}


