base_module = 
{
	module_type = "CoreMod.NoiseModule",
	params =
	{
		planet_width = 512,
		planet_height = 256
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
	module_type = "CoreMod.IntArrayVisualizer",
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
	module_type = "CoreMod.IntArrayVisualizer",
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