using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System;
using UIO;

namespace CoreMod
{
	public class CreationModifier
	{
		Scribe scribe = Scribes.Find ("Objects root");

		public Tag Tag { get; internal set; }

		ITable table;
		ICallback callback;

		public CreationModifier (ITable table, Tag tag)
		{
			Tag = tag;
			this.table = table;
			callback = table.GetCallback ("expression");
		}

	

		public void Apply (EntityComponent cmp)
		{
			callback.Call (cmp, table);
		}

	}

}

