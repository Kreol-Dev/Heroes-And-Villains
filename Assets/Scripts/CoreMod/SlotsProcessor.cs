
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;
using Demiurg.Core;


namespace CoreMod
{
    public abstract class SlotsProcessor : Demiurg.Core.Avatar
    {
        [AInput ("main")]
        protected List<GameObject> InputObjects;
        [AOutput ("main")]
        protected List<GameObject> OutputObjects;



    }
}
