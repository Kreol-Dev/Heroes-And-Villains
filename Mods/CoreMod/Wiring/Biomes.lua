regions_climate =
{
	avatar_type = "CoreMod.ClimateDataGatherer",
	inputs = {
		main = { "regions_slots_creator", "main" },
		temperature_map = { "temperature_finished", "main"},
		height_map = { "height_finished", "main" },
		inlandness_map = { "base_module", "main" },
		humidity_map = { "humidity_noise", "main" },
		radiation_map = { "radiation_noise", "main" }
	}
}

regions_tags =
{
	avatar_type = "CoreMod.TagsAssigner",
	configs =
	{
	},
	inputs =
	{
		main = { "regions_climate", "main" }
	}
}



regions_slots_creator = 
{
	avatar_type = "CoreMod.RegionsModule",
	configs =
	{
		name = "Land",
		density = 100,
		target_layer = defines.BIOMES_GO_LAYER
	},
	inputs =
	{
		main = { "surface_extractor", "extracted_chunks"}
	}
}

ocean_slots_creator = 
{
	avatar_type = "CoreMod.RegionsModule",
	configs =
	{
		name = "Ocean",
		density = 1000,
		target_layer = defines.BIOMES_GO_LAYER
	},
	inputs =
	{
		main = { "ocean_extractor", "extracted_chunks"}
	}
}

oceans_tags =
{
	avatar_type = "CoreMod.TagsAssigner",
	configs =
	{
	},
	inputs =
	{
		main = { "ocean_slots_creator", "main" }
	}
}
oceans_creator = 
{
	avatar_type = "CoreMod.SlotsReplacer",
	configs =
	{
				
	},
	inputs = 
	{
		main = { "oceans_tags", "main" }
	}
}

biomes_creator =
{
	avatar_type = "CoreMod.SlotsReplacer",
	configs =
	{
	},
	inputs = 
	{
		main = { "regions_tags", "main" }
	}
}