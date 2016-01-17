﻿using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Linq;
using System;


namespace DemiurgBinding
{
	[RootDependencies (typeof(ModsManager))]
	public class BindingRoot : Root
	{
		public ITabledRegistry Registry { get; internal set; }

		protected override void CustomSetup ()
		{
			Registry = new BindingRegistry ();
			Type attrType = typeof(ATabled);
			var tabledTypes = from type  in Find.Root<ModsManager> ().GetAllTypes ()
			                  where Attribute.GetCustomAttribute (type, attrType, true) != null
			                  select type;
			foreach (var type in tabledTypes)
				Registry.Register (type);
			Fulfill.Dispatch ();
		}



	}
}
