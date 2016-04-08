using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ListExt
{
	public static GCFreeListEnumerable<T> Iter<T> (this List<T> list)
	{
		return new GCFreeListEnumerable<T> (list);
	}
}

public struct GCFreeListEnumerable<T>
{
	private List<T> list;

	public GCFreeListEnumerable (List<T> aList)
	{
		list = aList;
	}

	public GCFreeListIterator<T> GetEnumerator ()
	{
		return new GCFreeListIterator<T> (list);
	}
}

public struct GCFreeListIterator<T>
{
	private int index;
	private List<T> list;

	public GCFreeListIterator (List<T> aList)
	{
		list = aList;
		index = -1;
	}

	public T Current {
		get { return list [index]; }
	}

	public bool MoveNext ()
	{
		return ++index < list.Count;
	}
}

