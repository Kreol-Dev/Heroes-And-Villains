﻿using Demiurg.Core.Extensions;
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

        public object Load (object fromObject, Type targetType, Demiurg.Core.ConfigLoaders loaders)
        {
            ITable objectTable = fromObject as ITable;
            object loadedObject = Activator.CreateInstance (targetType);
            LoadTemplate template = null;
            templates.TryGetValue (targetType, out template);
            if (template == null)
            {
                template = CreateTempalte (targetType);
                templates.Add (targetType, template);
            }
            foreach (var field in template.fields)
            {
                object value = objectTable.Get (field.Key);
                IConfigLoader loader = loaders.FindLoader (field.Type);
                if (value == null)
                {
                    scribe.LogFormatError ("Can't find value {0} in table {1} for object {2}", field.Key, fromObject, targetType);
                    continue;
                }
                if (loader == null)
                {
                    scribe.LogFormatError ("Can't find loader for a value {0} with type {1} in table {2} for object {3}", field.Key, field.Type, fromObject, targetType);
                    continue;
                }
                value = loader.Load (value, field.Type, loaders);
                //Call setter on the object with loaded value
                field.Setter (loadedObject, value);
            }
            return loadedObject;
        }


        LoadTemplate CreateTempalte (Type targetType)
        {
            LoadTemplate template = new LoadTemplate ();
            PropertyInfo[] fields = targetType.GetProperties (BindingFlags.Public);
            foreach (var field in fields)
            {
                var setter = BuildSetAccessor (field.GetSetMethod ());
                object[] attrs = field.GetCustomAttributes (true);
                object key;
                if (attrs.Length == 1)
                {
                    AConfig config = attrs [0] as AConfig;
                    key = config.Name;
                }
                else
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