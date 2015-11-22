using UnityEngine;
using System.Collections;
using System.Reflection;

namespace Demiurg.Core
{
    public class AvatarConfig : AvatarIO
    {
        public AvatarConfig (string name, FieldInfo field, Avatar avatar):base(name, field, avatar)
        {
        }
        public void SetValue (object value)
        {
            this.Field.SetValue (Avatar, value);
        }
    }
}