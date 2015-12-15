using Demiurg.Core.Extensions;
using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;

namespace Demiurg.Essentials
{
    public class CustomGenericLoader : IConfigLoader
    {

        #region IConfigLoader implementation

        public bool IsSpecific ()
        {
            return false;
        }

        public bool Check (Type targetType)
        {
            throw new NotImplementedException ();
        }

        public object Load (object fromObject, Type targetType, Demiurg.Core.ConfigLoaders loaders)
        {
            throw new NotImplementedException ();

        }

        #endregion

    }
}