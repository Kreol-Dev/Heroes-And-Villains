base_module = 
{
	avatar_type = "CoreMod.NoiseModule",
	configs =
	{
		width = defines.MAP_WIDTH,
		height = defines.MAP_HEIGHT,
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
		planet_connectivity = "sphere"
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
		levels =
		{
			{ 0, { 0,  0,  0}},
			{ 1, { 1,  1,  1}}
		}
		
		
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
	levels =
		{
			{ 0, { 0.3,  0.3,  0.8}},
			{ 1, { 0.3,  0.8,  0.3}}
		},
	random = false
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
		levels = {},
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
		tile_color = { 1, 1, 1}
	},
	inputs = 
	{
		texture = { "chunks_visual", "main" },
		tiles = { "random_points", "main" }
	}
}

latitude_temp_module =
{
	avatar_type = "CoreMod.LatitudeModule",
	configs =
	{
		polar_value = -50,
		central_value = 40,
		width = base_module.configs.width,
		height = base_module.configs.height
	}

}


temperature_noise = 
{
	avatar_type = "CoreMod.NoiseModule",
	configs =
	{
		width = base_module.configs.width,
		height = base_module.configs.height,
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
	levels =
	{
		{ -50, {0, 0, 1}},
		{ 50, {1, 0, 0}}
	}
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
	levels =
	{
		{ -50, {0, 0, 1}},
		{ 50, {1, 0, 0}}
	}
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
	levels=
	{
		{ -50, {0, 0, 1}},
		{ 50, {1, 0, 0}}
	}
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
		width = base_module.configs.width,
		height = base_module.configs.height,
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
		levels =
		{
		{ -4000, {0, 0, 1} },
		{ 0    , {0, 1, 1} },
		{ 4000 , {0.8, 0.8, 0} },
		{ 8000 , {1, 1, 1} }
	}
		
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
		tags_namespace = "climate"
	},
	inputs =
	{
		tags = { "tags_collection", "tags" },
		main = { "climate_gatherer", "main" }
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
				tags = { { "desert", 1 },  { "mountains", -1 }, { "plains", -1 } }
			},
			{ 
				ref = "plains",
				tags = { { "desert", -1 },  { "mountains", -1 }, { "plains", 1 } }
			},
			{ 
				ref = "mountains",
				tags = { { "desert", -1 },  { "mountains", 2 }, {  "plains", -1 } }
			}
		},
		tags_namespace = "climate",
		replacers_namespace = "cities"
		
	},
	inputs = 
	{
		main = { "climate_tags_assigner", "main" },
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