biomes_tiled_collection =
{
	collection_type = "CoreMod.GOCollection",
	layers = 
	{
		base_layer = "CoreMod.DefaultGOLayer",
		graphics_layer = "CoreMod.TiledGraphicsLayer",
		biomes_layer = "CoreMod.TiledBiomesLayer",
		--movement_layer = "CoreMod.TiledMovementLayer"
	}
}


statics_on_map_collection =
{
	collection_type = "CoreMod.GOCollection",
	layers = 
	{
		base_layer = "CoreMod.DefaultGOLayer",
		graphics_layer = "CoreMod.TiledGraphicsLayer",
		--population_layer = "CoreMod.TiledPopulationLayer",
		--hierarchy_layer = "CoreMod.TiledHierarchyLayer",
		encounters_layer = "CoreMod.TiledEncountersLayer"
	}
}

regions_collection = 
{
	collection_type = "CoreMod.GOCollection",
	layers = 
	{
		base_layer = "CoreMod.DefaultGOLayer",
		--hierarchy_layer = "CoreMod.TiledHierarchyLayer"
	}
}