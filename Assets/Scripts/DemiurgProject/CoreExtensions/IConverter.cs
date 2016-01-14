using UnityEngine;
using System.Collections;
using System;


namespace Demiurg.Core.Extensions
{
    public interface IConverter
    {
        bool IsSpecific ();
        bool Check (Type from, Type to);
        object Convert (object target, Converters converters);
    }
}


