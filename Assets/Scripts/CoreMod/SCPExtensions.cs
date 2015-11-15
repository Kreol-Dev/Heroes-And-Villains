using UnityEngine;
using System.Collections;
using Demiurg;


namespace CoreMod
{
    public static class SCPExtensionTile
    {
        public static SlotTile GetTile (this SlotComponentsProvider provider)
        {
            return provider.SlotGO.GetComponent<SlotTile> ();
        }
    }
    public static class SCPExtensionClimate
    {
        public static SlotClimate GetClimate (this SlotComponentsProvider provider)
        {
            return provider.SlotGO.GetComponent<SlotClimate> ();
        }
    }
}
