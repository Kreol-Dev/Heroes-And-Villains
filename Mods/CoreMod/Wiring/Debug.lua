base_noise_vis = 
{
	avatar_type = "CoreMod.FloatArrayVisualizer",
	configs =
	{
		levels = {{0, {0, 0, 0}}, {1, {1, 1, 1}}}
	},
	inputs = 
	{
		main = { "base_module", "main"}
	}
}

distinct_chunks_visualizer = 
{
	avatar_type = "CoreMod.DistinctArrayVisualizer",
	configs =
	{
		levels = {},
		random = true
	},
	inputs = 
	{
		main = { "distinctor_module", "main"}
	}
}

regions_visualizer = 
{
	avatar_type = "CoreMod.DistinctArrayVisualizer",
	configs =
	{
		levels = {},
		random = true
	},
	inputs = 
	{
		main = { "regions_slots_creator", "environment"}
	}
}
