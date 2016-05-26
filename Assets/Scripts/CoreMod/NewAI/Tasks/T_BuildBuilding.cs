using UnityEngine;
using System.Collections;
using NewAI;
using CoreMod;

public class T_BuildBuilding : Task<C_HasBuilding, J_BuildBuilding>
{
	BuildingType type;
	Agent agent;

	protected override void OnActionSucceed ()
	{
		PlannedAction = null;
	}

	protected override void OnActionFailed ()
	{
		PlannedAction = null;
	}

	protected override void InitAction (J_BuildBuilding jobAction)
	{
		jobAction.Setup (agent, type);
	}

	protected override void Setup (Agent agent, C_HasBuilding condition)
	{
		type = condition.BuildingType;
		this.agent = agent;

	}
	
}

public interface J_BuildBuilding
{
	void Setup (Agent agent, BuildingType type);
}

