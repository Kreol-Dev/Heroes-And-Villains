using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public interface IState_Production
	{
		int Production { get; set; }
	}

	public interface IState_Population
	{
		int Population { get; set; }
	}

	public interface IState_Tax
	{
		int Tax { get; set; }
	}

	public interface IState_Money
	{
		int Money { get; set; }
	}

	public interface IState_Stock
	{
		int Stock { get; set; }
	}
}

