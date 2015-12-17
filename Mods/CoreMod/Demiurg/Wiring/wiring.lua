base_module = 
{
	avatar_type = "CoreMod.NoiseModule",
	configs =
	{
		width = 64,
		height = 32,
		scale = 3
	}
}

distinctor_module = 
{
	avatar_type = "CoreMod.DistinctorModule",
	configs =
	{
		levels =
		{
		{ level = 0.6, val = 0},
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
		planet_connectivity = "full"
	},
	inputs = {
		main = { "distinctor_module", "main" }
	}
}

base_visual = 
{
	avatar_type = "CoreMod.FloatArrayVisualizer",
	configs = 
	{
		{ level = 0, red = 0, green = 0, blue = 0},
		{ level = 1, red = 1, green = 1, blue = 1}
		
	},
	inputs =
	{
		main = { "base_module", "main" }
	}
}

distinct_visual = 
{
	avatar_type = "CoreMod.DistinctArrayVisualizer",
	configs = 
	{
		{ level = 0, red = 0.3, green = 0.3, blue = 0.8},
		{ level = 1, red = 0.3, green = 0.8, blue = 0.3}
	},
	inputs =
	{
		main = { "distinctor_module", "main" }
	}
}

chunks_visual = 
{
	avatar_type = "CoreMod.DistinctArrayVisualizer",
	configs = 
	{
		random = true
	},
	inputs =
	{
		main = { "continents_module", "assignments" }
	}
}


surface_extractor =
{


	avatar_type = "CoreMod.ExtractSurfaceFromChunks",
	configs =
	{
		target_surface = 1,
		filter_less = 10
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
		density = 1000
	},
	inputs = 
	{
		main = { "surface_extractor", "main" }
	}
}

points_visualizer =
{

	avatar_type = "CoreMod.PointsVisualizer",
	configs =
	{
		tile_red = 1,
		tile_green = 1,
		tile_blue = 1
	},
	inputs = 
	{
		base_texture = { "chunks_visual", "main" },
		tiles = { "random_points", "main" }
	}
}

latitude_temp_module =
{
	avatar_type = "CoreMod.LatitudeModule",
	configs =
	{
		north_value = -50,
		central_value = 40,
		width = base_module.configs.planet_width,
		height = base_module.configs.planet_height
	}

}


temperature_noise = 
{
	avatar_type = "CoreMod.NoiseModule",
	configs =
	{
		planet_width = base_module.configs.planet_width,
		planet_height = base_module.configs.planet_height,
		scale = 2
	}
}

array_temperature_noise =
{
	avatar_type = "CoreMod.FloatToIntArray",
	configs = {
		max_value = 50,
		min_value = -50
	},
	inputs = 
	{
		main = { "temperature_noise", "main" }
	}
}

temperature_blend =
{
	avatar_type = "CoreMod.ArrayBlendModule",
	configs =
	{
		weight = 0.8
	},
	inputs = 
	{
		first = { "latitude_temp_module", "main" },
		second = { "array_temperature_noise", "main" }
	}
}
lat_visual = 
{
	avatar_type = "CoreMod.IntArrayVisualizer",
	configs = 
	{
		{ level = -50, red = 0, green = 0, blue = 1},
		{ level = 50, red = 1, green = 0, blue = 0}
	},
	inputs =
	{
		main = { "latitude_temp_module", "main" }
	}
}
temp_noise_visual = 
{
	avatar_type = "CoreMod.IntArrayVisualizer",
	configs = 
	{
		{ level = -50, red = 0, green = 0, blue = 1},
		{ level = 50, red = 1, green = 0, blue = 0}
	},
	inputs =
	{
		main = { "array_temperature_noise", "main" }
	}
}
temperature_visual = 
{
	avatar_type = "CoreMod.IntArrayVisualizer",
	configs = 
	{
		{ level = -50, red = 0, green = 0, blue = 1},
		{ level = 50, red = 1, green = 0, blue = 0}
	},
	inputs =
	{
		main = { "temperature_blend", "main" }
	}
}

height_noise = 
{
	avatar_type = "CoreMod.NoiseModule",
	configs =
	{
		planet_width = base_module.configs.planet_width,
		planet_height = base_module.configs.planet_height,
		scale = 3
	}
}

array_height_noise =
{
	avatar_type = "CoreMod.FloatToIntArray",
	configs = {
		max_value = 8000,
		min_value = -4000
	},
	inputs = 
	{
		main = { "height_noise", "main" }
	}
}

height_visual = 
{
	avatar_type = "CoreMod.IntArrayVisualizer",
	configs = 
	{
		{ level = -4000, red = 0, green = 0, blue = 1},
		{ level = 0, red = 0, green = 1, blue = 1},
		{ level = 4000, red = 0.8, green = 0.8, blue = 0},
		{ level = 8000, red = 1, green = 1, blue = 1}
	},
	inputs =
	{
		main = { "array_height_noise", "main" }
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
climate_gatherer =
{
	avatar_type = "CoreMod.ClimateDataGatherer",
	inputs = 
	{
		main = { "cities_placer", "slots" },
		temperature_map = { "temperature_blend", "main"},
		height_map = { "array_height_noise", "main" },
		inlandness_map = { "base_module", "main"}
	}
}

climate_tags_assigner =
{
	avatar_type = "CoreMod.TagsAssigner",
	configs =
	{
		tags = {
		{ tag_name = "climate_desert" },
		{ tag_name = "climate_mountains" },
		{ tag_name = "climate_plains" }

		}
	},
	inputs =
	{
		main = { "climate_gatherer", "main" }
	}
}


cities_creator = 
{
	avatar_type = "CoreMod.SlotsReplacer",
	configs =
	{
		{ 
			replacer = "desert_city",
			tags = { { tag_name = "climate_desert", weight = 1 },  { tag_name = "climate_mountains", weight = -1 }, { tag_name = "climate_plains", weight = -1 } }
		},
		{ 
			replacer = "plains_city",
			tags = { { tag_name = "climate_desert", weight = -1 },  { tag_name = "climate_mountains", weight = -1 }, { tag_name = "climate_plains", weight = 1 } }
		},
		{ 
			replacer = "mountains_city",
			tags = { { tag_name = "climate_desert", weight = -1 },  { tag_name = "climate_mountains", weight = 1 }, { tag_name = "climate_plains", weight = -1 } }
		}
	},
	inputs = 
	{
		main = { "climate_tags_assigner", "main" }
	}
}