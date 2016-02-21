biome_modifier = function ( biome, modifier )
	biome.MovementCost = biome.MovementCost + modifier.add_move_cost
	biome.Name = modifier.add_prefix .. " " .. biome.Name
end