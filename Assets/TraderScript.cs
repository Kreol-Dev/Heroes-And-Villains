using UnityEngine;
using System.Collections;
using CoreMod;

public class TraderScript : MonoBehaviour
{
	public City TargetCity;
	public City OriginCity;
	public float Speed;
	public Resource Resource;
	public Resource TradeFor;
	bool traded = false;
	public bool Finished = false;

	void Update ()
	{
		
		if (!traded)
		{
			var dirToTarget = TargetCity.transform.position - this.transform.position;
			var dist = dirToTarget.magnitude;
			if (dist < Speed * Time.deltaTime)
				return;
			dirToTarget /= dist;
			transform.position += dirToTarget * Time.deltaTime * Speed;
			//this.transform.LookAt (dirToTarget);

			Quaternion rotation = Quaternion.LookRotation
				(dirToTarget, transform.TransformDirection (Vector3.up));
			transform.rotation = new Quaternion (0, 0, rotation.z, rotation.w);

			transform.Rotate (new Vector3 (0, 0, 180));
		} else
		{
			var dirToTarget = OriginCity.transform.position - this.transform.position;
			var dist = dirToTarget.magnitude;
			dirToTarget /= dist;
			if (dist < Speed * Time.deltaTime)
				return;
			transform.position += dirToTarget * Time.deltaTime * Speed;
			//this.transform.LookAt (dirToTarget);
			Quaternion rotation = Quaternion.LookRotation
				(dirToTarget, transform.TransformDirection (Vector3.up));
			transform.rotation = new Quaternion (0, 0, rotation.z, rotation.w);
//			transform.Rotate (new Vector3 (0, 0, 180));
		}
	}

	void Start ()
	{

		Invoke ("FinishAuto", 30f);
	}

	void FinishAuto ()
	{
		Finished = true;
		Invoke ("DestroySelf", 5f);
	}

	void DestroySelf ()
	{
		Destroy (gameObject);
	}


	void OnTriggerEnter2D (Collider2D other)
	{
		if (!traded)
		{
			var city = other.gameObject.GetComponent<City> ();
			if (city != null)
			{
				if (city == TargetCity)
				{
					var tradeWith = city.FindRes (Resource.Type);
					var tradeFor = city.FindRes (TradeFor.Type);
					var money = tradeWith.Cost * Resource.Count;
					var amount = money / tradeFor.Cost;
					var fullAmount = (int)Mathf.Floor (amount);
					var tradedFor = Mathf.Min (fullAmount, tradeFor.Count);
					var resultCost = tradedFor * tradeFor.Cost;
					var tradedWithAmount = resultCost / tradeWith.Cost;
					var fullWithAmount = Mathf.CeilToInt (tradedWithAmount);

					Resource.Count -= fullWithAmount;
					tradeWith.Count += fullWithAmount;
					tradeFor.Count -= tradedFor;
					TradeFor.Count += tradedFor;
					traded = true;
				}
			}
		} else
		{
			var city = other.gameObject.GetComponent<City> ();
			if (city != null)
			{
				if (city == OriginCity)
				{
					city.FindRes (TradeFor.Type).Count += TradeFor.Count;
					city.FindRes (Resource.Type).Count += Resource.Count;
					Finished = true;
				}
			}

		}

	}
}
