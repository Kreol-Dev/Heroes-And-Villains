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
		size = 0,
		is_region = true,
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
		size = 0,
		is_region = true,
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
	inputs =
	{
		main = { "ocean_slots_creator", "main" }
	}
}
oceans_creator = 
{
	avatar_type = "CoreMod.SlotsReplacer",
	inputs = 
	{
		main = { "oceans_tags", "main" }
	}
}

biomes_creator =
{
	avatar_type = "CoreMod.SlotsReplacer",
	inputs = 
	{
		main = { "regions_tags", "main" }
	}
}