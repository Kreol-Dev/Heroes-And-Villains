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

    IConfigLoader floatLoader = null;
    Type floatType = typeof(float);

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
        if (floatLoader == null)
            floatLoader = loaders.FindLoader (floatType);
        Color color = new Color ((float)floatLoader.Load (red, floatType, loaders), 
                          (float)floatLoader.Load (green, floatType, loaders), 
                          (float)floatLoader.Load (blue, floatType, loaders));
        Debug.LogFormat ("COLOR LOADED {0}", color);
        return color;
    }



}

