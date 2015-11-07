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
		random = false,
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