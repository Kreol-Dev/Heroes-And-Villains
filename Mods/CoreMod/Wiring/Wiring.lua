
base_module = 
{
	avatar_type = "CoreMod.NoiseModule",
	configs =
	{
		east_edge_levels = { { 0, 1, 0} },
		west_edge_levels = { { 0, 1, 1} },
		south_edge_levels = { { 0, 0.5, 1}, {0.6, 1, 0} },
		north_edge_levels = { { 0, 1, 1} },
		corners = {
			north_east = 0,
			south_east = 0,
			north_west = 1,
			south_west = 1
		},
		scale = 3
	}
}

temperature_noise = 
{
	avatar_type = "CoreMod.NoiseModule",
	configs = base_module.configs
}

height_noise = 
{
	avatar_type = "CoreMod.NoiseModule",
	configs = base_module.configs
}
height_sharpen = 
{
	avatar_type = "CoreMod.MultiplyFloatArray",
	inputs =
	{
		array = { "height_noise", "main"},
		mod_array = { "height_noise", "main"}
	}	
}

height_sharpen_x2 = 
{
	avatar_type = "CoreMod.MultiplyFloatArray",
	inputs =
	{
		array = { "height_sharpen", "main"},
		mod_array = { "height_sharpen", "main"}
	}	
}

temperature_height_correction = 
{
	avatar_type = "CoreMod.SubMulFloatArray",
	inputs = 
	{
		array = { "temperature_noise", "main"},
		mod_array = { "height_sharpen_x2", "main"}
	}
}


humidity_noise =
{
	avatar_type = "CoreMod.NoiseModule",
	configs = base_module.configs
}

radiation_noise =
{
	avatar_type = "CoreMod.NoiseModule",
	configs = base_module.configs
}


temperature_finished =
{
	avatar_type = "CoreMod.TransposeByValueFloatArray",
	configs = {
		min_value = 0,
		max_value = 50
	},
	inputs = {
		array = { "temperature_height_correction", "main" }
	}
}

height_finished =
{
	avatar_type = "CoreMod.TransposeByValueFloatArray",
	configs = {
		min_value = 0,
		max_value = 4000
	},
	inputs = {
		array = { "height_sharpen_x2", "main" }
	}
}



cities_climate =
{
	avatar_type = "CoreMod.ClimateDataGatherer",
	inputs = 
	{
		main = { "cities_placer", "slots" },
		temperature_map = { "temperature_finished", "main"},
		height_map = { "height_finished", "main" },
		inlandness_map = { "base_module", "main" },
		humidity_map = { "humidity_noise", "main" },
		radiation_map = { "radiation_noise", "main" }
	}
}

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

distinctor_module = 
{
	avatar_type = "CoreMod.DistinctorModule",
	configs =
	{
		levels =
		{
		{ level = 0.3, val = 0},
		{ level = 1, val = 1}
	}
		
	},
	inputs = {
		main = { "base_module", "main" }
	}
}

continents_module = 
{
	avatar_type = "CoreMod.ContinuousChunksModule",
	configs = {
		planet_connectivity = "none"
	},
	inputs = {
		main = { "distinctor_module", "main" }
	}
}

surface_extractor =
{


	avatar_type = "CoreMod.ExtractSurfaceFromChunks",
	configs =
	{
		target_surface = 1,
		filter_less = 0
	},
	inputs =
	{
		main = { "continents_module", "chunks"}
	}
}

ocean_extractor =
{


	avatar_type = "CoreMod.ExtractSurfaceFromChunks",
	configs =
	{
		target_surface = 0,
		filter_less = 0
	},
	inputs =
	{
		main = { "continents_module", "chunks"}
	}
}


random_points = 
{

	avatar_type = "CoreMod.RandomPointsOnTiles",
	configs =
	{
		density = 300,
		min_count = 1
	},
	inputs = 
	{
		main = { "surface_extractor", "main" }
	}
}





cities_placer =
{
	avatar_type = "CoreMod.SlotsPlacer",
	inputs = 
	{
		points = {"random_points", "main"}
	}	
}


cities_climate_tags =
{
	avatar_type = "CoreMod.TagsAssigner",
	configs =
	{
		tags_namespace = "climate"
	},
	inputs =
	{
		tags = { "tags_collection", "tags" },
		main = { "cities_climate", "main" }
	}
}

regions_climate_tags =
{
	avatar_type = "CoreMod.TagsAssigner",
	configs =
	{
		tags_namespace = "climate"
	},
	inputs =
	{
		tags = { "tags_collection", "tags" },
		main = { "regions_climate", "main" }
	}
}



cities_creator = 
{
	avatar_type = "CoreMod.SlotsReplacer",
	configs =
	{
		replacers = 
		{
			{ 
				ref = "desert",
				tags = { { "desert", 10 }, { "mountains", -1 }, { "steppe", -5 } }
			},
			{ 
				ref = "plains",
				tags = { { "desert", -3 }, { "mountains", -1 }, { "plains" , 1 }, { "grassland", 4 }, { "irradiated", -5 } }
			},
			{ 
				ref = "oasis",
				tags = { { "desert", 1 },  { "mountains", -1 }, { "plains", 3 } }
			},
			{ 
				ref = "mountains",
				tags = { { "mountains", 1 }, { "hills", 3 }, { "swamp", -5 }, { "plains", -5 } }
			}
		},
		tags_namespace = "climate",
		replacers_namespace = "cities"
		
	},
	inputs = 
	{
		main = { "cities_climate_tags", "main" },
		available_tags = { "tags_collection", "tags"},
		available_replacers = { "replacers_collection", "replacers"}
	}
}



tags_collection =
{
	avatar_type = "CoreMod.TagsCollectionModule",
	configs =
	{
		tags_table = "tags"
	}
}

replacers_collection =
{
	avatar_type = "CoreMod.ReplacersCollectionModule",
	configs = 
	{
		replacers_table = "replacers"
	}
}

regions_slots_creator = 
{
	avatar_type = "CoreMod.RegionsModule",
	configs =
	{
		name = "Land",
		density = 100
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
		density = 1000
	},
	inputs =
	{
		main = { "ocean_extractor", "extracted_chunks"}
	}
}

oceans_creator = 
{
	avatar_type = "CoreMod.SlotsReplacer",
	configs =
	{
		replacers = 
		{
			{ 
				ref = "ocean_biome",
				tags = { { "ocean", 0 } }
			}
		},
		tags_namespace = "climate",
		replacers_namespace = "biomes"
		
	},
	inputs = 
	{
		main = { "ocean_slots_creator", "main" },
		available_tags = { "tags_collection", "tags"},
		available_replacers = { "replacers_collection", "replacers"}
	}
}

biomes_creator =
{
	avatar_type = "CoreMod.SlotsReplacer",
	configs =
	{
		replacers = 
		{
			{ 
				ref = "desert_biome",
				tags = { { "desert", 3 }, { "mountains", -1 } }
			},
			{ 
				ref = "mountains_biome",
				tags = { { "swamp", -5 }, { "mountains", 5 } }
			},
			{ 
				ref = "swamp_biome",
				tags = { { "desert", -3 }, { "mountains", -1 }, { "swamp", 5 } }
			},
			{ 
				ref = "hills_biome",
				tags = { { "mountains", 1 }, { "hills", 3 }, { "swamp", -5 } }
			},
			{ 
				ref = "steppe_biome",
				tags = { { "desert", 1 }, { "hills", -2 }, { "steppe", 5 }, { "mountains", -3 } }
			},
			{ 
				ref = "grassland_biome",
				tags = { { "desert", -3 }, { "mountains", -1 }, { "plains" , 1 }, { "grassland", 3 }, { "irradiated", -5 } }
			},
			{ 
				ref = "wasteland_biome",
				tags = { { "plains", 1 }, { "irradiated", 1 } }
			}
		},
		tags_namespace = "climate",
		replacers_namespace = "biomes"
		
	},
	inputs = 
	{
		main = { "regions_climate_tags", "main" },
		available_tags = { "tags_collection", "tags"},
		available_replacers = { "replacers_collection", "replacers"}
	}
}