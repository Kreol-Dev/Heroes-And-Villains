
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ModDesc
{
    public class SharedLibrary
    {
        public string Name { get; set; }

        public string Directory { get; set; }

        public List<string> Dependencies { get; set; }
    }

    public string Name { get; set; }

    public string Description { get; set; }

    public List<SharedLibrary> SharedLibraries { get; set; }

    public List<string> Dependencies { get; set; }

    public ModDesc ()
    {

    }
}




