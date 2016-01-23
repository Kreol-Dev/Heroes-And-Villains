cities ={}
cities.desert =
{
	settlement = 
	{
		population = 100,
		race = "desert people"
	},
	graphics = 
	{
		sprite = {"city_strip", "desert_city"}
	}
}

cities.plains =
{
	settlement = 
	{
		population = 200,
		race = "plains people"
	},
	graphics = 
	{
		sprite = {"city_strip", "plains_city"}
	}
}

cities.mountains =
{
	settlement = 
	{
		population = 50,
		race = "mountains people"
	},
	graphics = 
	{
		sprite = {"city_strip", "mountains_city"}
	}
}

cities.oasis =
{
	settlement = 
	{
		population = 50,
		race = "oasis people"
	},
	graphics = 
	{
		sprite = {"city_strip", "oasis_city"}
	}
}


provinces = {}

provinces.basic_province = 
{
	province = {}
}

biomes = {}
biomes.wasteland_biome = 
{
	biome = 
	{
		name = "Wasteland",
		priority = 1,
		tile_movement_cost = 1,
		tile_graphics = "wasteland"
	}
}
biomes.desert_biome = 
{
	biome = 
	{
		name = "Desert",
		priority = 1,
		tile_movement_cost = 1,
		tile_graphics = "desert"
	}
}