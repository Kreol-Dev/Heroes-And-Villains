using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using System.Collections.Generic;
using System;
using UIO;

namespace UIOBinding
{
	public class BindingTable : ITable
	{
		



		public Table Table { get; internal set; }

		public object Name { get; internal set; }

		public BindingTable (Table table, object name)
		{
			this.Table = table;
			this.Name = name;
		}

		#region get

		public int GetInt (object id)
		{
			object content = Table [id];
			if (content == null)
				throw new ITableMissingID (this.Name, id);
			else
				try
				{
					int value = (int)(double)content;
					return value;
				} catch (InvalidCastException e)
				{
					throw new ITableTypesMismatch (this.Name, id, content.GetType (), typeof(int));
				}
		}

		public float GetFloat (object id)
		{
			object content = Table [id];
			if (content == null)
				throw new ITableMissingID (this.Name, id);
			else
				try
				{
					float value = (float)(double)content;
					return value;
				} catch (InvalidCastException e)
				{
					throw new ITableTypesMismatch (this.Name, id, content.GetType (), typeof(float));
				}
		}

		public double GetDouble (object id)
		{
			object content = Table [id];
			if (content == null)
				throw new ITableMissingID (this.Name, id);
			else
				try
				{
					double value = (double)content;
					return value;
				} catch (InvalidCastException e)
				{
					throw new ITableTypesMismatch (this.Name, id, content.GetType (), typeof(double));
				}
		}

		public bool GetBool (object id)
		{
			object content = Table [id];
			if (content == null)
				throw new ITableMissingID (this.Name, id);
			else
				try
				{
					bool value = (bool)content;
					return value;
				} catch (InvalidCastException e)
				{
					throw new ITableTypesMismatch (this.Name, id, content.GetType (), typeof(bool));
				}
		}

		public string GetString (object id)
		{
			object content = Table [id];
			if (content == null)
				throw new ITableMissingID (this.Name, id);
			else
				try
				{
					string value = (string)content;
					return value;
				} catch (InvalidCastException e)
				{
					throw new ITableTypesMismatch (this.Name, id, content.GetType (), typeof(string));
				}
		}

		public ITable GetTable (object id)
		{
			object content = Table [id];
			if (content == null)
				throw new ITableMissingID (this.Name, id);
			else
				try
				{
					Table table = (Table)content;
					BindingTable value = new BindingTable (table, id);
					return value;
				} catch (InvalidCastException e)
				{
					throw new ITableTypesMismatch (this.Name, id, content.GetType (), typeof(ITable));
				}
		}

		public ICallback GetCallback (object id)
		{
			object content = Table [id];
			if (content == null)
				throw new ITableMissingID (this.Name, id);
			else
				try
				{
					Closure closure = (Closure)content;
					BindingFunction value = new BindingFunction (closure, id);
					return value;
				} catch (InvalidCastException e)
				{
					throw new ITableTypesMismatch (this.Name, id, content.GetType (), typeof(ICallback));
				}
		}

		#endregion

		#region nothrow

		public int GetInt (object id, int defaultValue)
		{
			try
			{
				return GetInt (id);
			} catch (ITableTypesMismatch e)
			{
				return defaultValue;
			} catch (ITableMissingID e)
			{
				return defaultValue;
			}
		}

		public float GetFloat (object id, float defaultValue)
		{
			try
			{
				return GetFloat (id);
			} catch (ITableTypesMismatch e)
			{
				return defaultValue;
			} catch (ITableMissingID e)
			{
				return defaultValue;
			}
		}

		public double GetDouble (object id, double defaultValue)
		{
			try
			{
				return GetDouble (id);
			} catch (ITableTypesMismatch e)
			{
				return defaultValue;
			} catch (ITableMissingID e)
			{
				return defaultValue;
			}
		}

		public bool GetBool (object id, bool defaultValue)
		{
			try
			{
				return GetBool (id);
			} catch (ITableTypesMismatch e)
			{
				return defaultValue;
			} catch (ITableMissingID e)
			{
				return defaultValue;
			}
		}

		public string GetString (object id, string defaultValue)
		{
			try
			{
				return GetString (id);
			} catch (ITableTypesMismatch e)
			{
				return defaultValue;
			} catch (ITableMissingID e)
			{
				return defaultValue;
			}
		}

		public ITable GetTable (object id, ITable defaultValue)
		{
			try
			{
				return GetTable (id);
			} catch (ITableTypesMismatch e)
			{
				return defaultValue;
			} catch (ITableMissingID e)
			{
				return defaultValue;
			}
		}

		public ICallback GetCallback (object id, ICallback defaultValue)
		{
			try
			{
				return GetCallback (id);
			} catch (ITableTypesMismatch e)
			{
				return defaultValue;
			} catch (ITableMissingID e)
			{
				return defaultValue;
			}
		}

		#endregion

		#region set

		public void Set (object id, int value)
		{
			Table [id] = value;
		}

		public void Set (object id, float value)
		{
			Table [id] = value;
		}

		public void Set (object id, double value)
		{
			Table [id] = value;
		}

		public void Set (object id, string value)
		{
			Table [id] = value;
		}

		public void Set (object id, bool value)
		{
			Table [id] = value;
		}

		public void Set (object id, ITable value)
		{
			BindingTable table = value as BindingTable;
			Table [id] = table.Table;
		}

		public void Set (object id, ICallback value)
		{
			BindingFunction callback = value as BindingFunction;
			Table [id] = callback.Closure;
		}

		#endregion

		#region else

		public IEnumerable GetKeys ()
		{
			List<object> keys = new List<object> ();
			foreach (var key in Table.Keys)
				keys.Add (key.ToObject ());
			return keys;
		}

		public bool Contains (object id)
		{
			return Table [id] != null;
		}

		#endregion
	}

}