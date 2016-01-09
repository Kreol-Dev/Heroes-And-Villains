using UnityEngine;
using System.Collections;
using System;
using Signals;
using MapRoot;

namespace CoreMod
{
    public interface IListMapLayer<T>
    {
        /*T GetByID (int id);

        T Find (Func<T, bool> criteria);

        int AddObject (T obj);*/

        Signal<T> ObjectAdded { get; }

        Signal<T> ObjectRemoved { get; }

        Signal<T> ObjectUpdated { get; }
    }

}