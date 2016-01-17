using UnityEngine;
using System.Collections;
using System;

namespace Demiurg.Core.Extensions
{
	[AttributeUsage (AttributeTargets.Struct | AttributeTargets.Class, Inherited = true)]
	public class ATabled : System.Attribute
	{
	}

	public interface ITabledRegistry
	{
		void Register (Type type);
	}
}


