coremod.farm =
{
	cost = { coremod_labour = 20},
	produces = { coremod_food = 10 },
	consumps = { coremod_labour = 2, coremod_materials = 2},
	name = "Farms",
	sprite = "farm"
}

coremod.factory =
{
	cost = { coremod_materials = 20, coremod_labour = 20},
	produces = { coremod_materials = 10 },
	consumps = { coremod_labour = 2, raw_material = 2, coremod_electricity = 2},
	name = "Factory",
	sprite = "factory"
}

coremod.houses = 
{
	cost = { coremod_materials = 20, coremod_labour = 20},
	produces = { coremod_housing = 10 },
	consumps = { coremod_electricity = 2 },
	name = "Farms",
	sprite = "house"
}

coremod.power_plant =
{
	cost = { coremod_materials = 20, coremod_labour = 20},
	produces = { coremod_electricity = 10 },
	consumps = { coremod_labour = 2, coremod_raw_materials = 2},
	name = "Power plant",
	sprite = "power_plant"
}

coremod.mine =
{
	cost = { coremod_labour = 20 },
	produces = { coremod_raw_materials = 10 },
	consumps = { coremod_labour = 2},
	name = "Mine",
	sprite = "mine"
}

coremod.workshop =
{
	cost = { coremod_raw_materials = 20, coremod_labour = 20 },
	produces = { coremod_materials = 2 },
	consumps = { coremod_labour = 2, coremod_raw_materials = 2},
	name = "Mine",
	sprite = "mine"
}
