coremod.farm =
{
	cost = { coremod_labour = 20},
	produces = { coremod_food = 10 },
	consumps = { coremod_labour = 2, coremod_materials = 2},
	name = "Farms",
	sprite = "farm",
	time = 10
}

coremod.factory =
{
	cost = { coremod_materials = 20, coremod_labour = 20},
	produces = { coremod_materials = 10 },
	consumps = { coremod_labour = 2, raw_material = 2, coremod_electricity = 2},
	name = "Factory",
	sprite = "factory",
	time = 15
}

coremod.houses = 
{
	cost = { coremod_materials = 20, coremod_labour = 20},
	produces = { coremod_housing = 10 },
	consumps = { coremod_electricity = 2 },
	name = "Farms",
	sprite = "house",
	time = 15
}

coremod.power_plant =
{
	cost = { coremod_materials = 20, coremod_labour = 20},
	produces = { coremod_electricity = 10 },
	consumps = { coremod_labour = 2, coremod_raw_materials = 2},
	name = "Power plant",
	sprite = "power_plant",
	time = 20
}

coremod.mine =
{
	cost = { coremod_labour = 20 },
	produces = { coremod_raw_materials = 10 },
	consumps = { coremod_labour = 2},
	name = "Mine",
	sprite = "mine",
	time = 10
}

coremod.workshop =
{
	cost = { coremod_raw_materials = 20, coremod_labour = 20 },
	produces = { coremod_materials = 2 },
	consumps = { coremod_labour = 2, coremod_raw_materials = 2},
	name = "Workshop",
	sprite = "workshop",
	time = 10
}
