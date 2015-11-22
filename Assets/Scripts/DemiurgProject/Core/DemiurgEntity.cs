using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Collections.Generic;
using System;

namespace Demiurg.Core
{
    public class DemiurgEntity
    {
        Dictionary<string, Avatar> avatars = new Dictionary<string, Avatar> ();
        Converters converters = null;
        ConfigLoaders loaders = null;
        public DemiurgEntity (Dictionary<string, Type> possibleAvatars, Dictionary<string, ITable> avatarsTables, List<IConverter> converters, List<IConfigLoader> loaders)
        {
            foreach (var avatar in avatarsTables)
            {
                Avatar newAvatar = NewAvatar (avatar.Key, avatar.Value, possibleAvatars);
                if (newAvatar == null)
                    continue;
                avatars.Add (newAvatar.Name, newAvatar);
            }
            this.converters = new Converters (converters);
            this.loaders = new ConfigLoaders (loaders);
        }

        Avatar NewAvatar (string name, ITable init, Dictionary<string, Type> possibleAvatars)
        {
            Type type = null;
            possibleAvatars.TryGetValue ((string)init.Get ("avatar_type"), out type);
            if (type == null)
                return null;
            return Avatar.Create (this, type, name, (ITable)init.Get ("inputs"), (ITable)init.Get ("configs"));
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

