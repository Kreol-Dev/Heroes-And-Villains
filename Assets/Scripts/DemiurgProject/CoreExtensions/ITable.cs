using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


namespace Demiurg.Core.Extensions
{
	public interface ITable
	{
		object Name { get; }

		IEnumerable GetKeys ();

		int GetInt (object id);

		float GetFloat (object id);

		double GetDouble (object id);

		bool GetBool (object id);

		string GetString (object id);

		ITable GetTable (object id);

		ICallback GetCallback (object id);

		int GetInt (object id, int defaultValue);

		float GetFloat (object id, float defaultValue);

		double GetDouble (object id, double defaultValue);

		bool GetBool (object id, bool defaultValue);

		string GetString (object id, string defaultValue);

		ITable GetTable (object id, ITable defaultValue);

		ICallback GetCallback (object id, ICallback defaultValue);

		bool Contains (object id);


		void Set (object id, int value);

		void Set (object id, float value);

		void Set (object id, double value);

		void Set (object id, string value);

		void Set (object id, bool value);

		void Set (object id, ITable value);

		void Set (object id, ICallback value);
	}


	public class ITableMissingID : Exception
	{
		object tableID;
		object id;

		public ITableMissingID (object tableID, object id)
		{
			this.tableID = tableID;
			this.id = id;
		}

		public override string ToString ()
		{
			return string.Format ("Missing id {0} in table {1}", id, tableID);
		}
	}

	public class ITableTypesMismatch : Exception
	{
		object tableID;
		object id;
		Type idType;
		Type targetType;

		public ITableTypesMismatch (object tableID, object id, Type idType, Type targetType)
		{
			this.id = id;
			this.tableID = tableID;
			this.idType = idType;
			this.targetType = targetType;
		}

		public override string ToString ()
		{
			return string.Format ("Types mismatch id {0} in table {1}, type in table is {2} while target type is {3}", id, tableID, idType, targetType);
		}
	}

}
