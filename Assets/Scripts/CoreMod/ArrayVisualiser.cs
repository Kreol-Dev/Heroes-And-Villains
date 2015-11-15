
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;
namespace CoreMod
{
    public abstract class ArrayVisualiser : CreationNode
    {
		
        protected class ColorLevel
        {
            public FloatParam Level = new FloatParam ("level");
            public FloatParam Red = new FloatParam ("red");
            public FloatParam Green = new FloatParam ("green");
            public FloatParam Blue = new FloatParam ("blue");
        }
        protected GlobalArrayParam<ColorLevel> Levels;
        protected override void SetupIOP ()
        {
            Levels = Config<GlobalArrayParam<ColorLevel>> ("levels");
        }
        protected sealed override void Work ()
        {
            GameObject go = new GameObject (Name);
            Map map = go.AddComponent<Map> ();
            map.Name = this.Name;
            map.Sprite = CreateSprite ();
            map.Setup ();
        }
        protected abstract Sprite CreateSprite ();
    }
}





