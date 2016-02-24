biome_modifier = function ( biome, modifier )
	biome.movement_cost = biome.movement_cost + modifier.add_move_cost
	biome.name = modifier.add_prefix .. " " .. biome.name
end