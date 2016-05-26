using UnityEngine;
using System.Collections;
using NewAI;
using CoreMod;

public interface J_TradeFor
{
	void TradeFor (ResourceType type, int amount);
}

public class A_Trade : NewAI.Action, J_TradeFor
{
	Agent agent;
	ResourceType type;
	int amount;
	TraderScript trader;

	public void TradeFor (ResourceType type, int amount)
	{
		this.type = type;
		this.amount = amount;
		trader = (GameObject.Instantiate (Resources.Load<GameObject> ("Trader"), agent.transform.position, Quaternion.identity) as GameObject).GetComponent<TraderScript> ();
		trader.OriginCity = agent.GetComponent<City> ();
		var originCity = agent.GetComponent<City> ();

		var cities = Object.FindObjectsOfType<City> ();
		float maxProfit = float.MinValue;
		City tradeWithCity = null;
		for (int i = 0; i < cities.Length; i++)
		{
			if (cities [i] != originCity)
			{
				var res = cities [i].FindRes (type);

				var dirToTarget = cities [i].transform.position - agent.transform.position;
				var dist = dirToTarget.sqrMagnitude;
				float profit = res.Count / res.Cost / (dist / 25);
				if (profit > maxProfit)
				{
					tradeWithCity = cities [i];
					maxProfit = profit;
				}
			}
		}
		Resource tradeRes = null;
		maxProfit = float.MinValue;
		for (int i = 0; i < tradeWithCity.resources.Count; i++)
		{
			var res = tradeWithCity.resources [i];
			var localResource = originCity.FindRes (res.Type);
			if (localResource.Count == 0)
				continue;
			float profit = res.Count * res.Cost * localResource.Count;
			if (profit > maxProfit)
			{
				tradeRes = tradeWithCity.resources [i];
				maxProfit = profit;
			}
		}
		if (tradeRes == null)
		{
			Debug.Log ("null resource");
			Object.Destroy (trader.gameObject);
			trader = null;
			return;

		}
		//Debug.Log (tradeRes);
		var localRes = originCity.FindRes (tradeRes.Type);
		localRes.Count /= 2;
		trader.TargetCity = tradeWithCity;
		trader.TradeFor = new Resource (){ Type = type };
		trader.Resource = new Resource (){ Type = tradeRes.Type, Count = localRes.Count / 2 };
	}

	public override void Update (System.Action onSuccess, System.Action onFail, Utilities uts)
	{

		if (trader == null)
		{
			Debug.Log ("Reinit trade");
			TradeFor (type, amount);
		} else if (trader.Finished)
		{
			Object.Destroy (trader.gameObject);
			trader = null;
			onSuccess ();
		}
	}

	public override float GetUtility (Utilities uts)
	{
		return 1f;
	}

	public override void Setup (Agent agent)
	{
		this.agent = agent;
	}

	public override void Prepare (int iteration)
	{
		
	}

	public override bool IsPossibleToPerformBy (GameObject go)
	{
		return go.GetComponent<City> () == null;
	}
}

