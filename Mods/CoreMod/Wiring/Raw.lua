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
