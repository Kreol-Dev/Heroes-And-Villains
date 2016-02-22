using UnityEngine;
using System.Collections;
using System;
using Signals;
using MapRoot;

namespace CoreMod
{
	public interface IListMapLayer<TObject> where TObject : class
	{
		event ObjectDelegate<TObject> ObjectAdded;
		event ObjectDelegate<TObject> ObjectRemoved;

		bool AddObject (TObject go);

		bool RemoveObject (TObject go);
	}

}