using UnityEngine;
using System.Collections;

using System.Reflection;
using System;


namespace Demiurg.Core
{
    public class AvatarIO
    {
        public object Name { get; internal set; }

        public string AvatarName { get { return Avatar.Name; } }

        protected Scribe Scribe = Scribes.Find ("DemiurgIO");
        Signals.Signal finishSignal = new Signals.Signal ();
        protected FieldInfo Field;
        protected Avatar Avatar;

        public AvatarIO (object name, FieldInfo field, Avatar avatar)
        {
            Field = field;
            Avatar = avatar;
            Name = name;
        }

        public Type FieldType ()
        {
            return Field.FieldType;
        }

        public object FieldValue ()
        {
            return Field.GetValue (Avatar);
        }

        public void OnFinish (Action callback)
        {
            finishSignal.AddOnce (callback);
        }

        protected void Finish ()
        {
            finishSignal.Dispatch ();
        }
    }
    
}
