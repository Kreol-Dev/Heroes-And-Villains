using UnityEngine;
using System.Collections;
using System.Reflection;


namespace Demiurg.Core
{
    public class AvatarOutput : AvatarIO
    {
        public AvatarOutput (object name, FieldInfo field, Avatar avatar) : base (name, field, avatar)
        {
        }

        public void Finish ()
        {
            base.Finish ();
        }
        
    }

}