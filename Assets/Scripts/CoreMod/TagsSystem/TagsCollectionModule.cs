using UnityEngine;
using System.Collections;
using Demiurg.Core;

namespace CoreMod
{
    public class TagsCollectionModule : Demiurg.Core.Avatar
    {
        [AConfig ("directory")]
        string directory;

        public override void Work ()
        {
            FinishWork ();
        }
    }

}

