
biomes_renderer_map =
{
	default_state = "Active",
	interactor = "biomes_interactor",
	layers = 
	{
		"biomes_layer"
	},
	renderer = "CoreMod.BiomesRenderer",
	renderer_data = 
	{
		priority = 0,
		atlas = "map_tiles",
		tiles = 
		{
			wasteland_tile = { sprite = "wasteland", priority = 1},
			red_tile = { sprite = "red", priority = 1},
			green_tile = { sprite = "green", priority = 1},
			mountains_tile = { sprite = "mountains", priority = 1},
	
			ocean_tile = { sprite = "ocean", priority = 0}
		},
		rules = 
		{
			type_to_tile = 
			{
				wasteland = "wasteland_tile",

				red = "red_tile",

				green = "green_tile",

				mountains = "mountains_tile",
				ocean = "ocean_tile"
			}
		}
	},
	presenter = "CoreMod.ObjectsPresenter",
	objectPresenter = "CoreMod.GOPresenter"
}
cities_rep =
{
	default_state = "Active",
	interactor = "cities_interactor",
	layers = 
	{
		"cities_layer"
	},
	renderer = "MapRoot.NullRenderer",
	renderer_data = {},
	presenter = "CoreMod.ObjectsPresenter",
	objectPresenter = "CoreMod.GOPresenter"
}


