using UnityEngine;
using System.Collections;
using Demiurg;
using MoonSharp.Interpreter;


namespace CoreMod
{
    [MoonSharpUserData]
    public class SlotClimate : SlotComponent
    {
        public int Height;
        public int Temperature;
        public int Humidity;
        public float Inlandness;
    }
}


