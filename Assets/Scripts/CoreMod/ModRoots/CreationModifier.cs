using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System;
using Demiurg.Core.Extensions;

namespace CoreMod
{
	public class Modifier
	{
		Scribe scribe = Scribes.Find ("Objects root");

		List<ModifierAtom> addAtoms;
		List<ModifierAtom> mulAtoms;

		public Tag Tag { get; internal set; }

		public Modifier (Type objectType, ITable table, Tag tag)
		{
			Tag = tag;
			addAtoms = SetupAtoms (objectType, table, "add");
			mulAtoms = SetupAtoms (objectType, table, "mul");
		}

		List<ModifierAtom> SetupAtoms (Type objectType, ITable table, string name)
		{
			List<ModifierAtom> atoms;
			try
			{
				atoms = GetAtomsFrom (objectType, table.GetTable (name));
			} catch
			{
				atoms = new List<ModifierAtom> ();
			}
			return atoms;
		}

		List<ModifierAtom> GetAtomsFrom (Type objectType, ITable table)
		{
			List<ModifierAtom> atoms = new List<ModifierAtom> ();
			foreach (var propertyName in table.GetKeys())
			{
				var field = objectType.GetField ((string)propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field == null)
				{
					scribe.LogFormatError ("Can't find field {0} in type {1}", propertyName, objectType);
					continue;
				}
				double value = table.GetDouble (propertyName);
				ModifierAtom atom = new ModifierAtom (){ Value = value, Field = field };
				atoms.Add (atom);
			}
			return atoms;
		}

		public void Apply (object toObject)
		{
			foreach (var atom in addAtoms)
			{
				object value = atom.Field.GetValue (toObject);
				if (value is float)
					atom.Field.SetValue (toObject, (float)value + (float)atom.Value);
				else if (value is int)
					atom.Field.SetValue (toObject, (int)value + (int)atom.Value);
				else if (value is double)
					atom.Field.SetValue (toObject, (double)value + atom.Value);
			}
			foreach (var atom in mulAtoms)
			{
				object value = atom.Field.GetValue (toObject);
				if (value is float)
					atom.Field.SetValue (toObject, (float)value * (float)atom.Value);
				else if (value is int)
					atom.Field.SetValue (toObject, (int)Math.Round ((double)value * atom.Value));
				else if (value is double)
					atom.Field.SetValue (toObject, (double)value * atom.Value);
			}
		}

		class ModifierAtom
		{
			public FieldInfo Field;
			public double Value;

		}

	}
}

