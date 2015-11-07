
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Scribes
{
    static Dictionary<string, Scribe> scribes = new Dictionary<string, Scribe> ();
    static Scribe scribe = new Scribe ("Logs\\Scribes.txt");
    public static Scribe Find (string scribeName)
    {
        Scribe scribe = null;
        scribes.TryGetValue (scribeName, out scribe);
        if (scribe == null)
        {
            Debug.Log ("Couldn't find scribe: " + scribeName);
            scribe = Register (scribeName);
        }
        return scribe;
    }

    public static void Save ()
    {
        foreach (var pair in scribes)
        {
            pair.Value.Save ();
        }
        scribe.Save ();
    }

    public static Scribe Register (string scribeName)
    {
        if (scribes.ContainsKey (scribeName))
        {
            scribe.Log ("Duplicate scribe: " + scribeName);
            return scribes [scribeName];
        }
        Scribe newScribe = new Scribe ("Logs\\" + scribeName + ".txt");
        scribes.Add (scribeName, newScribe);
        return newScribe;
    }
}




