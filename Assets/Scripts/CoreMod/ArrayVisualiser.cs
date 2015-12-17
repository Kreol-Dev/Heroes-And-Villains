
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;
using Demiurg.Core;


namespace CoreMod
{
    public abstract class ArrayVisualiser : Demiurg.Core.Avatar
    {
        protected class ColorLevel
        {
            [AConfig (1)]
            public float Level { get; set; }

            [AConfig (2)]
            public Color Color { get; set; }
        }

        [AConfig ("levels")]
        protected List<ColorLevel> Levels;

        public sealed override void Work ()
        {
            GameObject go = new GameObject (Name);
            Map map = go.AddComponent<Map> ();
            map.Name = this.Name;
            map.Sprite = CreateSprite ();
            map.Setup ();
            FinishWork ();
        }

        protected abstract Sprite CreateSprite ();
    }
}





