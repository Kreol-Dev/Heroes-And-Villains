using UnityEngine;
using System.Collections;
using Demiurg.Core;
using Demiurg.Core.Extensions;

namespace CoreMod
{
	public class CustomFloatModifierByArray : ArrayModifierByArray<float>
	{
		[AConfig ("modify_func")]
		ICallback modifyCallback;

		protected override float Modify (int x, int y, float value, float modValue)
		{
			return (float)(double)modifyCallback.Call (x, y, value, modValue);
		}


	}

	public class CustomFloatModifier : ArrayModifier<float>
	{
		[AConfig ("modify_func")]
		ICallback modifyCallback;

		protected override float Modify (int x, int y, float value)
		{
			return (float)(double)modifyCallback.Call (x, y, value);
		}


	}


}
