using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Demiurg.Core.Extensions;


namespace Demiurg.Core
{
    public class Converters
    {
        List<IConverter> converters = new List<IConverter> ();
        public Converters (List<IConverter> converters)
        {
            this.converters = converters;
        }
        public IConverter FindConverter (Type currentType, Type targetType)
        {
            IConverter converter = null;
            foreach (var conv in converters)
            {
                if (conv == null)
                {
                    if (conv.Check (currentType, targetType))
                    {
                        converter = conv;
                        if (conv.IsSpecific ())
                            break;
                    }
                    
                }
                else
                {
                    if (conv.IsSpecific ())
                    if (conv.Check (currentType, targetType))
                    {
                        converter = conv;
                        break;
                    }
                    
                }
                
            }
            
            return converter;
        }
    }
}


