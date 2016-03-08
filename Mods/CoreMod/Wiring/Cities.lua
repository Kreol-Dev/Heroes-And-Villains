settlements_plots =
{
	avatar_type = "CoreMod.PlotsGenerator",
	configs =
	{
		target_surface = defines.LAND_SURFACE,
		plots_count = defines.MAP_HEIGHT,
		groups = { groups = { "settlements"}, coremod = {"land"} }
	},
	inputs =
	{
		surfaces = { "distinctor_module", "main" }
	}	
}

settlements_tags =
{
	avatar_type = "CoreMod.TagsAssigner",
	inputs =
	{
		main = { "settlements_plots", "plots" }
	}
}

settlement_creators = 
{
	avatar_type = "CoreMod.SlotsReplacer",
	inputs = 
	{
		main = { "settlements_tags", "main" }
	}
}