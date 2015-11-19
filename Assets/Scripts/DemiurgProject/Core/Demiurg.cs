using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace Demiurg.Core
{
    public class Demiurg
    {
        Dictionary<string, Avatar> avatars = new Dictionary<string, Avatar> ();
        public Demiurg ()
        {

        }
        public Avatar FindAvatar (string name)
        {
            throw new System.NotImplementedException ();
        }

        public Converters GetConverters ()
        {
            throw new System.NotImplementedException ();
        }
    }

}

