
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDependency
{
	Signals.Signal Fulfill { get; }
}



