coremod.wasteland_biome = 
{
	creation =
	{
		availability = {coremod = {"land", "region"}},
		similarity = {coremod = {irradiated = 3 }}
	},
	entity = 
	{
		layer_name = "biomes_go_layer"
	},
	biome = 
	{
		biome_type = "wasteland",
		name = "Wasteland",
		tile_movement_cost = 1,
		modifiers = modifiers.biome
	},
	spatial_region = {}
}

coremod.green_biome = 
{
	creation =
	{
		availability = {coremod = { "land", "region"}},
		similarity = {coremod = {plains = 1 }}
	},
	entity = 
	{
		layer_name = "biomes_go_layer"
	},
	biome = 
	{
		biome_type = "green",
		name = "Green Biome",
		tile_movement_cost = 1,
		modifiers = modifiers.biome
	},
	spatial_region = {}
}

coremod.mountain_biome = 
{
	creation =
	{
		availability = {coremod = { "land", "region"}},
		similarity = {coremod = { mountains = 1 }}
	},
	entity = 
	{
		layer_name = "biomes_go_layer"
	},
	biome = 
	{
		biome_type = "mountains",
		name = "Mountains Biome",
		tile_movement_cost = 1,
		modifiers = modifiers.biome
	},
	spatial_region = {}
}

coremod.red_biome = 
{
	creation =
	{
		availability = {coremod = {"land", "region"}},
		similarity = {coremod = { desert = 1 } }
	},
	entity = 
	{
		layer_name = "biomes_go_layer"
	},
	biome = 
	{
		biome_type = "red",
		name = "Red Biome",
		tile_movement_cost = 1,
		modifiers = modifiers.biome
	},
	spatial_region = {}
}

coremod.ocean_biome =
{
	creation =
	{
		availability = {coremod = {"ocean", "region"}},
		similarity = {}
	},
	entity = 
	{
		layer_name = "biomes_go_layer"
	},
	biome = 
	{
		biome_type = "ocean",
		name = "Ocean",
		tile_movement_cost = 1,
		modifiers = {}
	},
	spatial_region = {}
}