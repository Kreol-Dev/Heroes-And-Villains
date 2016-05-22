biomes_go_layer =
{
	layer_type = "CoreMod.ObjectsLayer",
	configs = {}
}
biomes_layer =  
{ 
	layer_type = "CoreMod.RegionsLayer",
	configs = 
	{
		go_layer = "biomes_go_layer"
	}	
}

cities_layer =
{
	layer_type = "CoreMod.ObjectsLayer",
	configs = {}
}

surfaces_layer = 
{
	layer_type = "CoreMod.IntTileLayer",
	configs = {}
}

clearance_layer = 
{
	layer_type = "CoreMod.ClearanceLayer",
	configs = {}
}