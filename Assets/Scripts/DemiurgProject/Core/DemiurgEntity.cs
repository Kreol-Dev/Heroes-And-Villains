using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Collections.Generic;
using System;

namespace Demiurg.Core
{
    public class DemiurgEntity
    {
        Scribe scribe = Scribes.Find ("Demiurg");
        Dictionary<string, Avatar> avatars = new Dictionary<string, Avatar> ();
        Converters converters = null;
        ConfigLoaders loaders = null;

        public DemiurgEntity (Dictionary<string, Type> possibleAvatars, Dictionary<string, ITable> avatarsTables, List<IConverter> converters, List<IConfigLoader> loaders)
        {

            this.converters = new Converters (converters);
            this.loaders = new ConfigLoaders (loaders);
            foreach (var avatarType in possibleAvatars)
                Avatar.UseAvatarType (avatarType.Value);
            foreach (var avatar in avatarsTables)
            {
                Avatar newAvatar = NewAvatar (avatar.Key, avatar.Value, possibleAvatars);
                if (newAvatar == null)
                    continue;
                avatars.Add (newAvatar.Name, newAvatar);
            }
            foreach (var avatar in avatars)
            {
                ITable init = avatarsTables [avatar.Key];
                avatar.Value.Configure (this, (ITable)init.Get ("inputs"), (ITable)init.Get ("configs"));
            }
            foreach (var avatar in avatars)
            {
                avatar.Value.TryWork ();
            }
        }

        Avatar NewAvatar (string name, ITable init, Dictionary<string, Type> possibleAvatars)
        {
            Type type = null;
            object avType = init.Get ("avatar_type");
            if (avType == null)
            {
                scribe.LogFormatError ("{0} has no avatar type field while being considered an avatar", name);
                return null;
            }
                
            possibleAvatars.TryGetValue ((string)avType, out type);
            if (type == null)
            {
                scribe.LogFormatError ("{0} avatar type {1} is unaccessible", name, (string)avType);
                return null;
            }
                
            return Avatar.Create (type, name);
        }

        public Avatar FindAvatar (string name)
        {
            Avatar avatar = null;
            avatars.TryGetValue (name, out avatar);
            return avatar;
        }

        public Converters GetConverters ()
        {
            return converters;
        }

        public ConfigLoaders GetLoaders ()
        {
            return loaders;
        }
    }

}

