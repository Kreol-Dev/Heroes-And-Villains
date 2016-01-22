using UnityEngine;
using System.Collections;
using System;

namespace Demiurg.Core.Extensions
{
	[AttributeUsage (AttributeTargets.Struct | AttributeTargets.Class, Inherited = true)]
	public class AShared : System.Attribute
	{
	}

	public interface ITabledRegistry
	{
		void Register (Type type);
	}
}


