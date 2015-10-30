base_module = 
{
	module_type = "CoreMod.NoiseModule",
	params =
	{
		planet_width = 256,
		planet_height = 128
	},
	outputs = Outputs("base_module", module_type)
}

distinctor_module = 
{
	module_type = "CoreMod.DistinctorModule",
	outputs = Outputs("distinctor_module", module_type),
	params =
	{
		{ level = 0.6, val = 0},
		{ level = 1, val = 1}
	},
	inputs = {
		MainInput = base_module.outputs.MainOutput
	}
}

continents_module = 
{
	module_type = "CoreMod.ContinuousChunksModule",
	outputs = Outputs("continents_module", module_type),
	params = {
		chunks = { 1 },
		planet_connectivity = "full"
	},
	inputs = {
		MainInput = continents_module.outputs.MainOutput
	}
}