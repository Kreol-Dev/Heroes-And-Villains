using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DemiurgBinding;
using Demiurg.Core.Extensions;
using System.Reflection;

public class Mod
{
    public Dictionary<string, ITable> Tables = new Dictionary<string, ITable> ();
    public Assembly ModAssembly;

}

