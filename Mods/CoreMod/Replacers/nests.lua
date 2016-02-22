coremod.mutants_nest = 
{
	creation = 
	{
		availability = {coremod = {"land", "nest"}},
		similarity = {}
	},
	monsters_nest =
	{
		monster_name = "mutant",
		count = 10,
		food = 20, 
		has_chief = false,
		danger = 1,
		modifiers = 
		{
			coremod =
			{
				irradiated = 
				{
					add_count = 10,
					expression = function ( slot, modifier )
						slot.Count = slot.Count + modifier.add_count
					end
				},
				plains = 
				{
					add_count = -8,
					expression = function ( slot, modifier )
						slot.Count = slot.Count + modifier.add_count
					end
				}
			}
		}
	}
}