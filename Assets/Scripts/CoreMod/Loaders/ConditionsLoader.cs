using UnityEngine;
using System.Collections;
using UIO;
using AI;
using System;

namespace CoreMod
{
	public class ConditionsLoader : IConverter<Condition>
	{
		public override object Load (object key, ITable table, bool reference)
		{
			var aiRoot = Find.Root<AI.AIRoot> ();
			var mm = Find.Root<ModsManager> ();
			var condition = aiRoot.GetCondition (key as string);
			if (condition == null)
			{
				Scribes.Find ("CONVERTERS").LogFormatError ("No such condition: {0}", key);
				return null;
			}
			mm.Defs.LoadObject<Condition> (condition, table.GetTable (key));
			return condition;
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}

	}

	public class ModifiersLoader : IConverter<Modifier>
	{
		public override object Load (object key, ITable table, bool reference)
		{
			var modifiersRoot = Find.Root<Modifiers> ();
			var modifier = modifiersRoot.GetModifier (key as string);
			if (modifier == null)
			{
				Scribes.Find ("CONVERTERS").LogFormatError ("No such condition: {0}", key);
				return null;
			}
			//modifier.LoadFromTable (key, table);
			return modifier;
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}

	}


	public class NumSpecLoader : IConverter<NumericConditionSpec>
	{

		public override object Load (object key, ITable table, bool reference)
		{
			int code = table.GetInt (key);
			try
			{
				NumericConditionSpec spec = (NumericConditionSpec)code;
				return spec;
			} catch
			{

				return NumericConditionSpec.Equal;
			}
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}
	}

	public class ClassSpecLoader : IConverter<ClassConditionSpec>
	{
		public override object Load (object key, ITable table, bool reference)
		{
			int code = table.GetInt (key);
			if (code == 0)
				return ClassConditionSpec.Equal;
			else
				return ClassConditionSpec.NotEqual;
			
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}
	}
}
