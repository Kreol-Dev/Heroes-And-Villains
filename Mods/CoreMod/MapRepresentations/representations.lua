
tile_map_collection_representation =
{
	graphics_layer =
	{
		simple = { "CoreMod.GraphicsLayerRenderer", "CoreMod.GraphicsLayerPresenter", "CoreMod.GraphicsObjectPresenter" , "Active" }
	}
}

tiled_objects_collection_representation =
{
	base_layer = 
	{
		settlement = { "CoreMod.DefaultRenderer", "CoreMod.SettlementLayerPresenter", "CoreMod.SettlementPresenter", "Active"},
		encounter = { "CoreMod.DefaultRenderer", "CoreMod.EncounterLayerPresenter", "CoreMod.EncounterPresenter", "Active"}
	}
	
}

biomes_tiled_collection_representation =
{
	biomes_layer = 
	{
		graphics = { "CoreMod.BiomesRenderer", "CoreMod.BiomesLayerPresenter", "CoreMod.SettlementPresenter", "Active"}
	}
	
}