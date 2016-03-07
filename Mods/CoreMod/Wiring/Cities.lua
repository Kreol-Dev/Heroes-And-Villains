blocks_plots =
{
	avatar_type = "CoreMod.PlotsGenerator",
	configs =
	{
		target_surface = defines.LAND_SURFACE,
		plots_count = 30,
		groups = { groups = { "settlements"}, coremod = {"land"} }
	},
	inputs =
	{
		surfaces = { "distinctor_module", "main" }
	}	
}