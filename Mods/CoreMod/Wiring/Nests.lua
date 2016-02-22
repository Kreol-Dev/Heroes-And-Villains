nests_climate =
{
	avatar_type = "CoreMod.ClimateDataGatherer",
	inputs = {
		main = { "nests_slots_creator", "main" },
		temperature_map = { "temperature_finished", "main"},
		height_map = { "height_finished", "main" },
		inlandness_map = { "base_module", "main" },
		humidity_map = { "humidity_noise", "main" },
		radiation_map = { "radiation_noise", "main" }
	}
}

nests_tags =
{
	avatar_type = "CoreMod.TagsAssigner",
	inputs =
	{
		main = { "nest_slot_assigner", "main" }
	}
}



nests_slots_creator = 
{
	avatar_type = "CoreMod.RegionsModule",
	configs =
	{
		name = "Nest",
		size = 5,
		is_region = false,
		density = 100,
		target_layer = "nests_layer"
	},
	inputs =
	{
		main = { "surface_extractor", "extracted_chunks"}
	}
}

nests_creator =
{
	avatar_type = "CoreMod.SlotsReplacer",
	inputs = 
	{
		main = { "nests_tags", "main" }
	}
}

nest_slot_assigner = 
{
	avatar_type = "CoreMod.NestsSlotsModule",
	inputs = 
	{
		main = {"nests_climate", "main"}
	}
}