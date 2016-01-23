climate = {}
climate.desert =
{
	criteria = 
	{
		min_temperature = 30,
		max_temperature = 50,
		min_humidity = 0,
		max_humidity = 0.5
	},
	expression = expressions.desert_expression
}

climate.mountains =
{
	criteria = 
	{
		min_height = 800,
		max_height = 4000
	},
	expression = expressions.height_expression
}

climate.hills =
{
	criteria = 
	{
		min_height = 300,
		max_height = 800
	},
	expression = expressions.height_expression
}

climate.plains =
{
	criteria = 
	{
		min_temperature = 0,
		max_temperature = 30,
		min_height = 0,
		max_height = 300
	},
	expression = expressions.plains_expression
}

climate.irradiated =
{
	criteria = 
	{
		min_radioactivity = 0.5,
		max_radioactivity = 1
	},
	expression = expressions.rad_expression
}

climate.swamp =
{
	criteria = 
	{
		min_temperature = 20,
		max_temperature = 35,
		min_height = 0,
		max_height = 300,
		min_humidity = 0.5,
		max_humidity = 1
	},
	expression = expressions.swamp_expression
}

climate.steppe =
{
	criteria = 
	{
		min_temperature = 0,
		max_temperature = 40,
		min_height = 0,
		max_height = 300,
		min_humidity = 0,
		max_humidity = 0.5,
		min_inlandness = 0.5,
		max_inlandness = 1
	},
	expression = expressions.steppe_expression
}

