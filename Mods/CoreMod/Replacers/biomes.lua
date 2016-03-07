coremod.wasteland_biome = 
{
	creation =
	{
		availability = {coremod = {"land", "region"}},
		similarity = {}
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