coremod.workers =
{
	chance = 20,
	transit_to = { coremod_capitalists = 1, coremod_officials = 2, coremod_soldiers = 5},
	produces = { coremod_labour = 10 },
	consumps = { coremod_food = 1, coremod_housing = 1},
	name = "Workers",
	sprite = "workers"
}

coremod.capitalists =
{
	chance = 1,
	produces = {  },
	transit_to = {},
	consumps = { coremod_labour = 2, coremod_food = 3, coremod_electricity = 2, coremod_housing = 3},
	name = "Capitalists",
	sprite = "capitalists"
}

coremod.officials = 
{
	chance = 2,
	produces = {  },
	transit_to = {},
	consumps = { coremod_food = 2, coremod_electricity = 2, coremod_housing = 2 },
	name = "Officials",
	sprite = "officials"
}

coremod.soldiers = 
{
	chance = 10,
	produces = { },
	transit_to = {},
	consumps = { coremod_food = 2, coremod_electricity = 2, coremod_housing = 1 },
	name = "Soldiers",
	sprite = "soldiers"
}