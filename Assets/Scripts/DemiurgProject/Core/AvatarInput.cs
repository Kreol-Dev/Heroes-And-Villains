using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using Demiurg.Core.Extensions;


namespace Demiurg.Core
{
    public class AvatarInput : AvatarIO
    {
        public AvatarInput (string name, FieldInfo field, Avatar avatar):base(name, field, avatar)
        {
        }
        public void ConnectTo (AvatarOutput output, Converters converters)
        {
            Type outType = output.FieldType ();
            Type inType = this.FieldType ();
            if (outType != inType)
            {
                Scribe.LogFormatWarning ("IO TYPES MISMATCH Input {0} {1} {2} and Output {3} {4} {5}\n Will try to find converter", Avatar.Name, Name, inType, output.AvatarName, output.Name, outType);
                IConverter converter = converters.FindConverter (outType, outType);
                if (converter == null)
                {
                    Scribe.LogFormatError ("IO TYPES MISMATCH Input {0} {1} {2} and Output {3} {4} {5}\n Converter not found", Avatar.Name, Name, inType, output.AvatarName, output.Name, outType);
                    return;
                }
                output.OnFinish (() => {
                    Scribe.LogFormat ("IO OUTPUT->INPUT Input {0} {1} and Output {2} {3}", Avatar.Name, Name, output.AvatarName, output.Name);
                    this.Field.SetValue (Avatar, converter.Convert (output.FieldValue ()));
                    base.Finish ();});
                return;
            }    
            Scribe.LogFormat ("IO CONNECTION Input {0} {1} {2} and Output {3} {4} {5}", Avatar.Name, Name, inType, output.AvatarName, output.Name, outType);
            output.OnFinish (() => {
                Scribe.LogFormat ("OUTPUT->INPUT Input {0} {1} and Output {2} {3}", Avatar.Name, Name, output.AvatarName, output.Name);
                this.Field.SetValue (Avatar, output.FieldValue ());
                base.Finish ();});
        }
    }

}