using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Collections.Generic;
using System;


namespace Demiurg.Core
{
    public class ConfigLoaders
    {
        List<IConfigLoader> loaders = new List<IConfigLoader> ();
        public ConfigLoaders (List<IConfigLoader> loaders)
        {
            this.loaders = loaders;
        }
        public IConfigLoader FindLoader (Type targetType)
        {
            IConfigLoader loader = null;
            foreach (var load in loaders)
            {
                if (load == null)
                {
                    if (load.Check (targetType))
                    {
                        loader = load;
                        if (load.IsSpecific ())
                            break;
                    }
                    
                }
                else
                {
                    if (load.IsSpecific ())
                    if (load.Check (targetType))
                    {
                        loader = load;
                        break;
                    }
                    
                }
                
            }
            
            return loader;
        }

    }

}