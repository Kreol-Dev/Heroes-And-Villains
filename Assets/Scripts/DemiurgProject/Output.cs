
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Demiurg
{
	public class Output : IDependency
	{
		public Signals.Signal Fulfill {	get ; internal set; }


	}

	public class Output<T> : Output
	{
		public T Content;
		public static implicit operator T(Output<T> output)
		{
			return output.Content;
		}
	}
}



