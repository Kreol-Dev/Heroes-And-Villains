cities_climate =
{
	avatar_type = "CoreMod.ClimateDataGatherer",
	inputs = 
	{
		main = { "cities_placer", "slots" },
		temperature_map = { "temperature_finished", "main"},
		height_map = { "height_finished", "main" },
		inlandness_map = { "base_module", "main" },
		humidity_map = { "humidity_noise", "main" },
		radiation_map = { "radiation_noise", "main" }
	}
}

random_points = 
{

	avatar_type = "CoreMod.RandomPointsOnTiles",
	configs =
	{
		density = 300,
		min_count = 0
	},
	inputs = 
	{
		main = { "surface_extractor", "main" }
	}
}


cities_placer =
{
	avatar_type = "CoreMod.SlotsPlacer",
	configs =
	{
		target_layer = defines.STATIC_OBJECTS_LAYER
	},
	inputs = 
	{
		points = {"random_points", "main"}
	}	
}

cities_climate_tags =
{
	avatar_type = "CoreMod.TagsAssigner",
	configs =
	{
	},
	inputs =
	{
		main = { "cities_climate", "main" }
	}
}

cities_creator = 
{
	avatar_type = "CoreMod.SlotsReplacer",
	configs =
	{
		
		
	},
	inputs = 
	{
		main = { "cities_climate_tags", "main" }
	}
}

encounter_points = 
{
	avatar_type = "CoreMod.RandomPointsOnTiles",
	configs = 
	{
		density = 20,
		min_count = 0 
	},
	inputs =
	{
		main = { "surface_extractor", "main" }
	}
}

encounter_placer =
{
	avatar_type = "CoreMod.SlotsPlacer",
	configs =
	{
		target_layer = defines.STATIC_OBJECTS_LAYER
	},
	inputs = 
	{
		points = {"encounter_points", "main"}
	}
}

encounter_creator =
{
	avatar_type = "CoreMod.SlotsReplacer",
	configs =
	{
		
	},
	inputs = 
	{
		main = { "encounter_placer", "slots" }
	}
}