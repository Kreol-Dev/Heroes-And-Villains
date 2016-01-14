using UnityEngine;
using System.Collections;
using Demiurg;
using Demiurg.Core;

namespace CoreMod
{
    public class ClimateDataGatherer : SlotsProcessor
    {
        [AInput ("inlandness_map")]
        float[,] inlandnessMap;
        [AInput ("temperature_map")]
        int[,] temperatureMap;
        [AInput ("height_map")]
        int[,] heightMap;
        /*protected override void SetupIOP ()
        {
            base.SetupIOP ();
            temperatureMap = Input<int[,]> ("temperature_map");
            heightMap = Input<int[,]> ("height_map");
            inlandnessMap = Input<float[,]> ("inlandness_map");
        }

        protected override void Work ()
        {
            foreach (var go in InputObjects.Content)
            {
                SlotTile tile = go.GetComponent<SlotTile> ();
                SlotClimate climate = go.AddComponent<SlotClimate> ();
                climate.Height = heightMap.Content [tile.X, tile.Y];
                climate.Temperature = temperatureMap.Content [tile.X, tile.Y];
                climate.Inlandness = inlandnessMap.Content [tile.X, tile.Y];
                climate.Humidity = 0;
            }
            if (InputObjects == null)
                Scribe.LogFormatError ("Null GObjects list in {0}", Name);
            OutputObjects.Finish (InputObjects.Content);
        }*/
        public override void Work ()
        {
            OutputObjects = InputObjects;
            FinishWork ();
        }
    }
}


