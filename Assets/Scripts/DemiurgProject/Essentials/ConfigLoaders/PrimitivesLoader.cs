using Demiurg.Core.Extensions;
using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;

namespace Demiurg.Essentials
{
    public class PrimitivesLoader : IConfigLoader
    {
        static Type intType = typeof(int);
        static Type stringType = typeof(string);
        static Type boolType = typeof(bool);
        static Type floatType = typeof(float);

        #region IConfigLoader implementation

        public bool IsSpecific ()
        {
            return true;
        }

        public bool Check (Type targetType)
        {
            return (targetType == intType || targetType == stringType || targetType == boolType || targetType == floatType);
        }

        public object Load (object fromObject, Type targetType, Demiurg.Core.ConfigLoaders loaders)
        {
            if (targetType == stringType)
            {
                if (fromObject is DynValue)
                    return ((DynValue)fromObject).ToPrintString ();
            }
            if (targetType == intType)
            {
                if (fromObject is DynValue)
                    return (int)((DynValue)fromObject).CastToNumber ().Value;
            }
            if (targetType == boolType)
            {
                if (fromObject is DynValue)
                    return ((DynValue)fromObject).CastToBool ();
            }
            if (targetType == floatType)
            {
                if (fromObject is DynValue)
                    return (float)((DynValue)fromObject).CastToNumber ().Value;
            }
            return fromObject;
        }

        #endregion

    }
}

