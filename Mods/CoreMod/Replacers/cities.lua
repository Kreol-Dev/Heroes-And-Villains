coremod.large_city =
{
	creation =
	{
		availability = { groups = {"settlements"}, coremod = {"land"}},
		similarity = {},
		fixed_space = { size = 3, form = "circle" }
	},
	entity = 
	{
		layer_name = "cities_layer"
	},
	planner = 
	{
		risky = 0.5,
		bold = 0.5,
		concentrated_on_result = 0.5
	},
	agent = {},
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
		race = "humans"
	},
	spatial_material = 
	{
		form = 
		{
			object_type = "CoreMod.CircleForm",
			radius = 1.5,
			center = { 0, 0 }
		}
	}
}

coremod.middle_city =
{
	creation =
	{
		availability = { groups = {"settlements"}, coremod = {"land"}},
		similarity = {},
		fixed_space = { size = 2, form = "circle" }
	},
	entity = 
	{
		layer_name = "cities_layer"
	},
	agent = {},
	planner = 
	{
		risky = 0.5,
		bold = 0.5,
		concentrated_on_result = 0.5
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
		race = "humans"
	},
	spatial_material = 
	{
		form = 
		{
			object_type = "CoreMod.CircleForm",
			radius = 1,
			center = { 0, 0 }
		}
	}
}


coremod.small_city =
{
	creation =
	{
		availability = { groups = {"settlements"}, coremod = {"land"}},
		similarity = {},
		fixed_space = { size = 1, form = "rect" }
	},
	entity = 
	{
		layer_name = "cities_layer"
	},
	agent = {},
	planner = 
	{
		risky = 0.5,
		bold = 0.5,
		concentrated_on_result = 0.5
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
		race = "humans"
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