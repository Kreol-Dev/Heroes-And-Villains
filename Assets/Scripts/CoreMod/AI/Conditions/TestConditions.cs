using UnityEngine;
using System.Collections;

namespace CoreMod
{
	[System.Serializable]
	public class C_Population : AI.Condition<C_Population, Settlement>
	{
		public int CurPopulation { get { return Component.Population; } set { Component.Population = value; } }

		public int TargetPopulation;
	}

	[System.Serializable]
	public class C_Wealth : AI.Condition<C_Wealth, Settlement>
	{
		public int CurWealth { get { return Component.Wealth; } set { Component.Wealth = value; } }

		public int TargetWealth;
	}

	[System.Serializable]
	public class C_Production : AI.Condition<C_Production, Settlement>
	{
		public int CurProduction { get { return Component.Production; } set { Component.Production = value; } }

		public int TargetProduction;
	}

	[System.Serializable]
	public class C_Food : AI.Condition<C_Food, Settlement>
	{
		public int CurFood { get { return Component.Food; } set { Component.Food = value; } }

		public int TargetFood;
	}
}

