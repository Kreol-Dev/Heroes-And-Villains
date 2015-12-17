using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg.Core.Extensions;
using System;
using MoonSharp.Interpreter;

namespace Demiurg.Essentials
{
    public class ArrayLoader : IConfigLoader
    {

        #region IConfigLoader implementation

        public bool IsSpecific ()
        {
            return false;
        }

        public bool Check (Type targetType)
        {
            return targetType.IsArray;
        }

        public object Load (object fromObject, Type targetType, Demiurg.Core.ConfigLoaders loaders)
        {
            List<object> objects = new List<object> ();
            Table table = fromObject as Table;
            Type containedType = targetType.GetGenericArguments () [0];
            IConfigLoader loader = loaders.FindLoader (containedType);
            var keys = table.Keys;
            foreach (var key in keys)
            {
                objects.Add (loader.Load (table [key], containedType, loaders));
            }
            return objects.ToArray ();

        }

        #endregion

    }
}
