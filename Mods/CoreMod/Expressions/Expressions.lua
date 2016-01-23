rad_expression = function (slot, criteria )
	climate = slot.Get(component.Climate)
	if climate.radioactivity >= criteria.min_radioactivity and climate.radioactivity <= criteria.max_radioactivity then
		return true	
	else
		return false
	end
end

height_expression = function (slot, criteria )
	climate = slot.Get(component.Climate)
	if climate.height >= criteria.min_height and climate.height <= criteria.max_height then
		return true	
	else
		return false
	end
end

temp_hum__expression = function (slot, criteria )
	climate = slot.Get(component.Climate)
	if climate.temperature >= criteria.min_temperature and climate.temperature <= criteria.max_temperature and 
		climate.humidity >= criteria.min_humidity and climate.humidity <= criteria.max_humidity then
		return true	
	else
		return false
	end
end

plains_expression = function (slot, criteria )
	climate = slot.Get(component.Climate)
	if climate.temperature >= criteria.min_temperature and climate.temperature <= criteria.max_temperature and 
		climate.height >= criteria.min_height and climate.height <= criteria.max_height then
		return true	
	else
		return false
	end
end

swamp_expression = function (slot, criteria )
	climate = slot.Get(component.Climate)
	if climate.temperature >= criteria.min_temperature and climate.temperature <= criteria.max_temperature and 
		climate.humidity >= criteria.min_humidity and climate.humidity <= criteria.max_humidity and
		climate.height >= criteria.min_height and climate.height <= criteria.max_height then
		return true	
	else
		return false
	end
end

steppe_expression = function (slot, criteria )
	climate = slot.Get(component.Climate)
	if climate.temperature >= criteria.min_temperature and climate.temperature <= criteria.max_temperature and 
		climate.humidity >= criteria.min_humidity and climate.humidity <= criteria.max_humidity and
		climate.inlandness >= criteria.min_inlandness and climate.inlandness <= criteria.max_inlandness and
		climate.height >= criteria.min_height and climate.height <= criteria.max_height then
		return true	
	else
		return false
	end
end