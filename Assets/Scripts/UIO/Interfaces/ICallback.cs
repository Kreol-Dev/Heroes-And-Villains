using UnityEngine;
using System.Collections;

namespace UIO
{
	public interface ICallback
	{
		object ID { get; }

		object Call (params object[] args);
	}

}


