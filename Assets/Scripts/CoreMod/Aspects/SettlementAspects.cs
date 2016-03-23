using UnityEngine;
using System.Collections;

namespace CoreMod
{
	[Aspect ("add_prod_mod")]
	public class ProdEffAddAspect : FloatAddAspect<Settlement, ProdEffState>
	{
		public override void LoadFrom (object key, UIO.ITable table)
		{
			Value = table.GetFloat (key, 0f);
		}
	}

	[Aspect ("mul_prod_mod")]
	public class ProdEffMulAspect : FloatMulAspect<Settlement, ProdEffState>
	{
		public override void LoadFrom (object key, UIO.ITable table)
		{
			Value = table.GetFloat (key, 1f);
		}
	}

	[Aspect ("mul_food_mod")]
	public class FoodEffMulAspect : FloatMulAspect<Settlement, FoodEffState>
	{
		public override void LoadFrom (object key, UIO.ITable table)
		{
			Value = table.GetFloat (key, 1f);
		}
	}

	[Aspect ("add_food_mod")]
	public class FoodEffAddAspect : FloatAddAspect<Settlement, FoodEffState>
	{
		public override void LoadFrom (object key, UIO.ITable table)
		{
			Value = table.GetFloat (key, 0f);
		}
	}


}

