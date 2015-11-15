using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using System.Linq;
using UnityEditor;
using MoonSharp.Interpreter.Interop;


namespace Demiurg
{
    [MoonSharpUserData]
    public class SlotComponentsProvider
    {
        [MoonSharpVisible(false)]
        public static List<Type>
            Types;
        public SlotComponent Get (int componentID)
        {
            if (SlotGO != null)
                return SlotGO.GetComponent (Types [componentID]) as SlotComponent;
            else
                return null;
        }
        [MoonSharpVisible(false)]
        public GameObject
            SlotGO;
    }

}

