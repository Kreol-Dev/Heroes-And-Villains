base_module = 
{
	module_type = "CoreMod.NoiseModule",
	params =
	{
		planet_width = 64,
		planet_height = 32,
		scale = 3
	}
}

distinctor_module = 
{
	module_type = "CoreMod.DistinctorModule",
	params =
	{
		{ level = 0.6, val = 0},
		{ level = 1, val = 1}
	},
	inputs = {
		main = { "base_module", "main" }
	}
}

continents_module = 
{
	module_type = "CoreMod.ContinuousChunksModule",
	params = {
		planet_connectivity = "full"
	},
	inputs = {
		main = { "distinctor_module", "main" }
	}
}

base_visual = 
{
	module_type = "CoreMod.FloatArrayVisualizer",
	params = 
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
	module_type = "CoreMod.DistinctArrayVisualizer",
	params = 
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
	module_type = "CoreMod.DistinctArrayVisualizer",
	params = 
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


	module_type = "CoreMod.ExtractSurfaceFromChunks",
	params =
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

	module_type = "CoreMod.RandomPointsOnTiles",
	params =
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

	module_type = "CoreMod.PointsVisualizer",
	params =
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
	module_type = "CoreMod.LatitudeModule",
	params =
	{
		north_value = -50,
		central_value = 40,
		width = base_module.params.planet_width,
		height = base_module.params.planet_height
	}

}


temperature_noise = 
{
	module_type = "CoreMod.NoiseModule",
	params =
	{
		planet_width = base_module.params.planet_width,
		planet_height = base_module.params.planet_height,
		scale = 2
	}
}

array_temperature_noise =
{
	module_type = "CoreMod.FloatToIntArray",
	params = {
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
	module_type = "CoreMod.ArrayBlendModule",
	params =
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
	module_type = "CoreMod.IntArrayVisualizer",
	params = 
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
	module_type = "CoreMod.IntArrayVisualizer",
	params = 
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
	module_type = "CoreMod.IntArrayVisualizer",
	params = 
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
	module_type = "CoreMod.NoiseModule",
	params =
	{
		planet_width = base_module.params.planet_width,
		planet_height = base_module.params.planet_height,
		scale = 3
	}
}

array_height_noise =
{
	module_type = "CoreMod.FloatToIntArray",
	params = {
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
	module_type = "CoreMod.IntArrayVisualizer",
	params = 
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
	module_type = "CoreMod.SlotsPlacer",
	inputs = 
	{
		points = {"random_points", "main"}
	}	
}
climate_gatherer =
{
	module_type = "CoreMod.ClimateDataGatherer",
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
	module_type = "CoreMod.TagsAssigner",
	params =
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
	module_type = "CoreMod.SlotsReplacer",
	params =
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