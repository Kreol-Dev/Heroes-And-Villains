climate_expression = function (slot, criteria )
	climate = slot.Get(component.Climate)
	if climate.temperature >= criteria.min_temperature and climate.temperature <= criteria.max_temperature then
		return true	
	else
		return false
	end
end