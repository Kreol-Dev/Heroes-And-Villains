biomes = {}
biomes.wasteland_biome = 
{
	creation =
	{
		availability = {surface = {"land"}, object = {"region"}},
		similarity = {}
	},
	biome = 
	{
		name = "Wasteland",
		priority = 1,
		tile_movement_cost = 1,
		tile_graphics = "wasteland",
		modifiers = modifiers.biome
	}
}

biomes.ocean_biome =
{
	creation =
	{
		availability = {surface = {"ocean"}, object = {"region"}},
		similarity = {}
	},
	biome = 
	{
		name = "Ocean",
		priority = 1,
		tile_movement_cost = 1,
		tile_graphics = "ocean",
		modifiers = {}
	}
}