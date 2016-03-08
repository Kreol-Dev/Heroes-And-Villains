
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
			ocean_tile = { sprite = "ocean", priority = 0}
		},
		rules = 
		{
			type_to_tile = 
			{
				wasteland = "wasteland_tile",
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


