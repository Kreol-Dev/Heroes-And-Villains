using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using Demiurg.Core.Extensions;
using System.Linq;

namespace Demiurg.Core
{
    public abstract class Avatar
    {
        Scribe scribe = Scribes.Find ("Avatars");
        public static Avatar Create (Demiurg demiurg, Type type, string name, ITable wiringTable, ITable configs)
        {
            Avatar avatar = Activator.CreateInstance (type) as Avatar;
            avatar.SetupIO ();
            avatar.Configure (demiurg, name, wiringTable, configs);
            return avatar;
        }
        protected List<AvatarInput> Inputs = new List<AvatarInput> ();
        protected Dictionary<string, AvatarOutput> Outputs = new Dictionary<string, AvatarOutput> ();
        public abstract void SetupIO ();

        public string Name { get; internal set; }
        protected Demiurg Demiurg { get; set; }
        public void Configure (Demiurg demiurg, string name, ITable wiringTable, ITable configs)
        {
            Name = name;
            SetupWiring (wiringTable);
            SetupConfigs (configs);
        }

        public AvatarOutput GetOutput (string name)
        {
            AvatarOutput output = null;
            Outputs.TryGetValue (name, out output);
            return output;
        }

        void SetupWiring (ITable wiringTable)
        {
            foreach (var input in Inputs)
            {
                ITable table = wiringTable.Get (input.Name) as ITable;
                if (table == null)
                {
                    scribe.LogFormatError ("INPUT DATA MISSING: Can't find wiring reference for avatar {0} input {1}", Name, input.Name);
                    continue;
                }
                string targetAvatarName = table.Get (1) as String;
                string targetOutputName = table.Get (2) as String;
                if (targetAvatarName == null || targetOutputName == null)
                {
                    scribe.LogFormatError ("INPUT DATA MISSING: Can't retrieve wiring reference data for avatar {0} input {1}\n Retrieved: {2} | {3}", Name, input.Name, targetAvatarName, targetOutputName);
                    continue;
                }
                Avatar targetAvatar = Demiurg.FindAvatar (targetAvatarName);
                if (targetAvatar == null)
                {
                    scribe.LogFormatError ("INPUT DATA MISSING: Can't find target avatar {2} for avatar {0} input {1}", Name, input.Name, targetAvatarName);
                    continue;
                }
                AvatarOutput output = targetAvatar.GetOutput (targetOutputName);
                if (output == null)
                {
                    scribe.LogFormatError ("INPUT DATA MISSING: Can't find output {2} in avatar {3} for avatar {0} input {1}", Name, input.Name, targetOutputName, targetAvatarName);
                    continue;
                }
                input.ConnectTo (output, Demiurg.GetConverters ());
            }
        }

        void SetupConfigs (ITable configs)
        {
            
        }

        public abstract void Work ();
        protected void FinishWork ()
        {
            foreach (var output in Outputs)
                output.Value.Finish ();
        }
    }

    public abstract class Avatar<T> : Avatar where T : Avatar
    {
        class FieldData
        {
            public FieldInfo Field { get; internal set; }
            public string ID { get; internal set; }
            public FieldData (FieldInfo field, string id)
            {
                Field = field;
                ID = id;
            }


        }
        static IEnumerable<FieldData> inputs;
        static IEnumerable<FieldData> outputs;
        static Avatar ()
        {
            Type inputAttr = typeof(Input);
            Type outputAttr = typeof(Output);
            Type type = MethodBase.GetCurrentMethod ().DeclaringType;
            var fields = type.GetFields ();
            inputs = from field in fields where field.IsDefined (inputAttr, false) select new FieldData (field, ((Input)Attribute.GetCustomAttribute (field, inputAttr)).Name);
            outputs = from field in fields where field.IsDefined (outputAttr, false) select new FieldData (field, ((Input)Attribute.GetCustomAttribute (field, inputAttr)).Name);
        }
        public sealed override void SetupIO ()
        {
            foreach (var inp in inputs)
            {
                AvatarInput input = new AvatarInput (inp.ID, inp.Field, this);
                Inputs.Add (input);
            }

            foreach (var outp in outputs)
            {
                AvatarOutput output = new AvatarOutput (outp.ID, outp.Field, this);
                Outputs.Add (outp.ID, output);
            }
        }
    }

}