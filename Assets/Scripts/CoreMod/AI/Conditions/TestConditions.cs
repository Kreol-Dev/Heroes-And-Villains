using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class C_Population : AI.Condition<C_Population, Settlement>
	{
		public int CurPopulation { get { return Component.Population; } set { Component.Population = value; } }

		public int TargetPopulation;
	}

	public class C_Wealth : AI.Condition<C_Wealth, Settlement>
	{
		public int CurWealth { get { return Component.Wealth; } set { Component.Wealth = value; } }

		public int TargetWealth;
	}

	public class C_Production : AI.Condition<C_Production, Settlement>
	{
		public int CurProduction { get { return Component.Production; } set { Component.Production = value; } }

		public int TargetProduction;
	}

	public class C_Food : AI.Condition<C_Food, Settlement>
	{
		public int CurFood { get { return Component.Food; } set { Component.Food = value; } }

		public int TargetFood;
	}
}

