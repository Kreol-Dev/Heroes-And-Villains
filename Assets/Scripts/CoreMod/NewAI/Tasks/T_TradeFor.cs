using UnityEngine;
using System.Collections;
using NewAI;
using CoreMod;

public class T_TradeFor : NewAI.Task<C_HasResource, J_TradeFor>
{
	ResourceType type;
	int Amount;
	Agent agent;

	protected override void OnActionSucceed ()
	{
		PlannedAction = null;
	}

	protected override void OnActionFailed ()
	{
		PlannedAction = null;
	}

	protected override void InitAction (J_TradeFor jobAction)
	{
		//Debug.Log ("init " + jobAction);
		jobAction.TradeFor (type, Amount);
	}

	protected override void Setup (Agent agent, C_HasResource condition)
	{

		//Debug.Log ("setup trade" + agent);
		type = condition.Type;
		Amount = condition.Resource;
		this.agent = agent;

	}



}

