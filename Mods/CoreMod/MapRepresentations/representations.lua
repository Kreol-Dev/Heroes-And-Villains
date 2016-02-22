biomes_renderer_map =
{
	default_state = "Active",
	interactor = "tiles_interactor",
	layers = 
	{
		"biomes_tiles_layer"
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
			wasteland_biome = "wasteland_tile",
			ocean_biome = "ocean_tile"
		}
	},
	presenter = "MapRoot.NullLayerPresenter",
	objectPresenter = "MapRoot.NullObjectPresenter"
}


nests_renderer_map =
{
	default_state = "Active",
	interactor = "tiles_interactor",
	layers = 
	{
		"nests_tiles_layer"
	},
	renderer = "CoreMod.NestsTilesRenderer",
	renderer_data = 
	{
		priority = 1,
		atlas = "nests",
		tiles = 
		{
			large_mutant_nest = { sprite = "large_nest", priority = 1},
			small_mutant_nest = { sprite = "small_nest", priority = 0}
		},
		rules = 
		{
			mutant = 
			{ 
				large_mutant_nest = { count = 10},
				small_mutant_nest = { count = 0 }
			}
		}
	},
	presenter = "MapRoot.NullLayerPresenter",
	objectPresenter = "MapRoot.NullObjectPresenter"
}

