coremod.large_block =
{
	creation =
	{
		availability = { groups = {"settlements"}, coremod = {"land"}},
		similarity = {},
		fixed_space = { size = 3, form = "circle" }
	},
	entity = 
	{
		layer_name = "blocks_layer"
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

coremod.small_block =
{
	creation =
	{
		availability = { groups = {"settlements"}, coremod = {"land"}},
		similarity = {},
		fixed_space = { size = 1, form = "rect" }
	},
	entity = 
	{
		layer_name = "blocks_layer"
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