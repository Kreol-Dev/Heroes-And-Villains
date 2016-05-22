coremod.large_city =
{
	creation =
	{
		availability = { groups = {"settlements"}, coremod = {"land"}},
		similarity = {}
	},
		structure = { size = 3, form = "Circle" },
	unit = 
	{
		surface = defines.LAND_SURFACE,
		creation_cost = 20
	},
	entity = 
	{
		layer_name = "cities_layer"
	},
	settlement =
	{
		base_food = 10,
		base_production = 10,
		population = 5,
		production_mod = 1,
		food_mod = 1,
		food = 10,
		production = 10,
		wealth = 100,
		race = "humans",
		city_image = "large_city"
	},
	city =
	{

	},
	spatial_material = 
	{
		form = 
		{
			object_type = "CoreMod.CircleForm",
			radius = 3,
			center = { 0, 0 }
		}
	}
}

coremod.middle_city =
{
	creation =
	{
		availability = { groups = {"settlements"}, coremod = {"land"}},
		similarity = {}
	},
	structure = { size = 2, form = "Circle" },
	unit = 
	{
		surface = defines.LAND_SURFACE,
		creation_cost = 15
	},
	entity = 
	{
		layer_name = "cities_layer"
	},
	settlement =
	{
		base_food = 10,
		base_production = 10,
		population = 3,
		production_mod = 1,
		food_mod = 1,
		food = 10,
		production = 10,
		wealth = 100,
		race = "humans",
		city_image = "middle_city"
	},
	city =
	{

	},
	spatial_material = 
	{
		form = 
		{
			object_type = "CoreMod.CircleForm",
			radius = 2,
			center = { 0, 0 }
		}
	}
}


coremod.small_city =
{
	creation =
	{
		availability = { groups = {"settlements"}, coremod = {"land"}},
		similarity = {}
	},

		structure = { size = 1, form = "Rect" },
	unit = 
	{
		surface = defines.LAND_SURFACE,
		creation_cost = 10
	},
	entity = 
	{
		layer_name = "cities_layer"
	},
	settlement =
	{
		base_food = 5,
		base_production = 5,
		population = 5,
		production_mod = 0.2,
		food_mod = 0.2,
		food = 10,
		production = 10,
		wealth = 5,
		race = "humans",
		city_image = "small_city"
	},
	city =
	{

	},
	spatial_material = 
	{
		form = 
		{
			object_type = "CoreMod.RectForm",
			size = { 1, 1 },
			center = { 0, 0 }
		}
	}
}