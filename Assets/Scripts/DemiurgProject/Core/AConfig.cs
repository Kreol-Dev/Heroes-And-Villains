using UnityEngine;
using System.Collections;
using System;
using UIO;

namespace Demiurg.Core
{
	public class AConfig : DefinedAttribute
	{
		public object Name { get; internal set; }

		public AConfig (string name) : base (name)
		{
			Name = name;
		}

		public AConfig (int id) : base (id)
		{
			Name = id;
		}
	}
}

