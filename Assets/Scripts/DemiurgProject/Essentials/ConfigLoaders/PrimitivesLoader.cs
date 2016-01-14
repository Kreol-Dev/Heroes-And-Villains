using Demiurg.Core.Extensions;
using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

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
            if (fromObject is DynValue)
            {
                if (targetType == stringType)
                {
                    return ((DynValue)fromObject).ToPrintString ();
                }
                if (targetType == intType)
                {

                    return (int)((DynValue)fromObject).CastToNumber ().Value;
                }
                if (targetType == boolType)
                {
                    return ((DynValue)fromObject).CastToBool ();
                }
                if (targetType == floatType)
                {
                    return (float)((DynValue)fromObject).CastToNumber ().Value;
                }
            }
            else
            {
                if (targetType == intType)
                {
                    return Convert.ToInt32 (fromObject);
                }
                if (targetType == floatType)
                {
                    return Convert.ToSingle (fromObject);
                }
            }
            Debug.Log (targetType.ToString () + " " + fromObject.GetType ().ToString ());
            return fromObject;
        }

        #endregion

    }
}

