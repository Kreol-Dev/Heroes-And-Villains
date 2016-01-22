using UnityEngine;
using System.Collections;
using Demiurg.Core;

namespace CoreMod
{
	public class MultiplyFloatArray : ArrayModifierByArray<float>
	{

		protected override float Modify (int x, int y, float value, float modValue)
		{
			return value * modValue;
		}
	}

	public class SubMulFloatArray : ArrayModifierByArray<float>
	{

		protected override float Modify (int x, int y, float value, float modValue)
		{
			return value - value * modValue;
		}
	}

	public class BlendFloatArray : ArrayModifierByArray<float>
	{
		[AConfig ("blend_value")]
		float blendValue;

		protected override float Modify (int x, int y, float value, float modValue)
		{
			return Mathf.Lerp (value, modValue, blendValue);
		}
	}

	public class TransposeByValueFloatArray : ArrayModifier<float>
	{
		[AConfig ("min_value")]
		float minValue;
		[AConfig ("max_value")]
		float maxValue;

		protected override float Modify (int x, int y, float value)
		{
			return Mathf.Lerp (minValue, maxValue, value);
		}
	}

}


