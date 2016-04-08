using UnityEngine;
using System.Collections;
using AI;

namespace CoreMod
{

	[Condition ("population")]
	public class C_Population : AI.IntCondition<C_Population, Settlement, PopulationState>
	{
		public override void LoadFromTable (object key, UIO.ITable table)
		{
			TargetValue = table.GetInt (key);
		}
	}


	[Condition ("wealth")]
	public class C_Wealth : AI.IntCondition<C_Wealth, Settlement, WealthState>
	{
		public override void LoadFromTable (object key, UIO.ITable table)
		{
			TargetValue = table.GetInt (key);
		}
	}


	[Condition ("production")]
	public class C_Production : AI.IntCondition<C_Production, Settlement, ProductionState>
	{
		public override void LoadFromTable (object key, UIO.ITable table)
		{
			TargetValue = table.GetInt (key);
		}
	}

	[Condition ("food")]
	public class C_Food : AI.IntCondition<C_Food, Settlement, FoodState>
	{
		public override void LoadFromTable (object key, UIO.ITable table)
		{
			TargetValue = table.GetInt (key);
		}
	}

	[Condition ("prod_mod")]
	public class C_ProdEfficiency : AI.FloatCondition<C_ProdEfficiency, Settlement, ProdEffState>
	{
		public override void LoadFromTable (object key, UIO.ITable table)
		{
			TargetValue = table.GetFloat (key);
		}
	}

	[Condition ("food_mod")]
	public class C_FoodEfficiency : AI.FloatCondition<C_FoodEfficiency, Settlement, FoodEffState>
	{
		public override void LoadFromTable (object key, UIO.ITable table)
		{
			TargetValue = table.GetFloat (key);
		}
	}
}

