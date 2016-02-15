
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