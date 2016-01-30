using Demiurg.Core.Extensions;
using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using System.Reflection;
using System.Linq.Expressions;
using Demiurg.Core;

namespace Demiurg.Essentials
{
	public class CustomGenericLoader : IConfigLoader
	{
		Scribe scribe = Scribes.Find ("Loaders");

		class LoadEntry
		{
			public Type Type;
			public object Key;
			public Action<object, object> Setter;
		}

		class LoadTemplate
		{
			public List<LoadEntry> fields = new List<LoadEntry> ();
		}

		static Dictionary<Type, LoadTemplate> templates = new Dictionary<Type, LoadTemplate> ();

		public bool IsSpecific ()
		{
			return false;
		}

		public bool Check (Type targetType)
		{
			return targetType.IsClass && !targetType.IsGenericType && !targetType.IsAbstract;
		}

		public object Load (ITable fromTable, object id, Type targetType, Demiurg.Core.ConfigLoaders loaders)
		{
			
			ITable objectTable = fromTable.GetTable (id);
			object loadedObject = Activator.CreateInstance (targetType);
			LoadTemplate template = null;
			templates.TryGetValue (targetType, out template);
			if (template == null)
			{
				template = CreateTemplate (targetType);
				templates.Add (targetType, template);
			}
			scribe.LogFormat ("Found values {0} in table {1}[{3}] for object {2}", template.fields.Count, fromTable.Name, targetType, id);
			foreach (var field in template.fields)
			{
//				object value = objectTable.Get (field.Key);
				IConfigLoader loader = loaders.FindLoader (field.Type);
//				if (value == null)
//				{
//					scribe.LogFormatError ("Can't find value {0} in table {1} for object {2}", field.Key, fromObject, targetType);
//					continue;
//				}
//				if (loader == null)
//				{
//					scribe.LogFormatError ("Can't find loader for a value {0} with type {1} in table {2} for object {3}", field.Key, field.Type, fromObject, targetType);
//					continue;
//				}
//				scribe.LogFormat ("Found value {0} with type {1} in table {2} for object {3}", field.Key, field.Type, fromObject, targetType);
				object value = loader.Load (objectTable, field.Key, field.Type, loaders);
				//Call setter on the object with loaded value
				field.Setter (loadedObject, value);
			}
			return loadedObject;
		}


		LoadTemplate CreateTemplate (Type targetType)
		{
			LoadTemplate template = new LoadTemplate ();
			PropertyInfo[] fields = targetType.GetProperties (BindingFlags.Public | BindingFlags.Instance);
			scribe.LogFormat ("Found fields {0} in type {1}", fields.Length, targetType);
			foreach (var field in fields)
			{
				var setter = BuildSetAccessor (field.GetSetMethod ());
				object[] attrs = field.GetCustomAttributes (true);
				object key;
				if (attrs.Length == 1)
				{
					AConfig config = attrs [0] as AConfig;
					key = config.Name;
				} else
					key = field.Name;
				template.fields.Add (new LoadEntry () {
					Key = key,
					Setter = setter,
					Type = field.PropertyType
				});
			}
			return template;
		}

		static Action<object, object> BuildSetAccessor (MethodInfo method)
		{
			var obj = Expression.Parameter (typeof(object), "o");
			var value = Expression.Parameter (typeof(object), "v");

			Expression<Action<object, object>> expr =
				Expression.Lambda<Action<object, object>> (
					Expression.Call (
						Expression.Convert (obj, method.DeclaringType),
						method,
						Expression.Convert (value, method.GetParameters () [0].ParameterType)),
					obj,
					value);

			return expr.Compile ();
		}
	}
}