using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class Find
{
    static Dictionary<Type, Root> roots = new Dictionary<Type, Root> ();
    static Scribe scribe = new Scribe ("Logs\\Roots.txt");
    public static T Root<T> () where T : Root
    {
        Root root = null;
        roots.TryGetValue (typeof(T), out root);
        if (root == null)
            scribe.Log ("Couldn't find root: " + typeof(T).ToString ());
        return root as T;
    }
    public static Root Root (Type rootType)
    {
        Root root = null;
        roots.TryGetValue (rootType, out root);
        if (root == null)
            scribe.Log ("Couldn't find root: " + rootType.ToString ());
        return root;
    }

    public static void Register<T> () where T : Root
    {
        Type rootType = typeof(T);
        if (roots.ContainsKey (rootType))
        {
            scribe.Log ("Duplicate root: " + rootType.ToString ());
            return;
        }
        GameObject go = new GameObject ("Root GO: " + rootType.ToString ());
        roots.Add (typeof(T), go.AddComponent<T> ());
    }


}

