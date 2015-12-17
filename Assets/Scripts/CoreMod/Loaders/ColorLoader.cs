using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System;

public class ColorLoader : IConfigLoader
{
    Type colorType = typeof(Color);

    public bool IsSpecific ()
    {
        return true;
    }

    public bool Check (System.Type targetType)
    {
        return targetType == colorType;
    }

    public object Load (object fromObject, System.Type targetType, Demiurg.Core.ConfigLoaders loaders)
    {
        ITable table = fromObject as ITable;
        if (table == null)
            return Color.clear;
        object red = table.Get (1);
        object green = table.Get (2);
        object blue = table.Get (3);
        if (red == null)
        {
            red = table.Get ("red");
            green = table.Get ("green");
            blue = table.Get ("blue");
        }
        return new Color ((float)red, (float)green, (float)blue);
    }



}

