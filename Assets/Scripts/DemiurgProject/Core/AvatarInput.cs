using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using UIO;


namespace Demiurg.Core
{
	public class AvatarInput : AvatarIO
	{
		public AvatarInput (object name, FieldInfo field, Avatar avatar) : base (name, field, avatar)
		{
		}

		public void ConnectTo (AvatarOutput output)
		{
			Type outType = output.FieldType ();
			Type inType = this.FieldType ();
			if (outType != inType)
			{
				Scribe.LogFormatWarning ("IO TYPES MISMATCH Input {0} {1} {2} and Output {3} {4} {5}\n Will try to find converter", Avatar.Name, Name, inType, output.AvatarName, output.Name, outType);
				return;
			}    
			Scribe.LogFormat ("IO CONNECTION Input {0} ({6}) {1} {2} and Output {3} ({7}) {4} {5}", Avatar.Name, Name, inType, output.AvatarName, output.Name, outType, Avatar.GetType (), output.GetAvatarType ());
			output.OnFinish (() =>
            {
				Scribe.LogFormat ("OUTPUT->INPUT Input {0}  ({4}) {1} and Output {2} ({5}) {3}", Avatar.Name, Name, output.AvatarName, output.Name, Avatar.GetType (), output.GetAvatarType ());
				Scribe.LogFormat ("OUTPUT->INPUT Output value {0}", output.FieldValue ());
				this.Field.SetValue (Avatar, output.FieldValue ());
				base.Finish ();
            });
		}
	}

}