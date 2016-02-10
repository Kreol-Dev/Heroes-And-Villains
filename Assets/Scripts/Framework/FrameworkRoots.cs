using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;

public class FrameworkRoots : MonoBehaviour
{
    void Awake ()
    {
        Find.Register<ConsoleRoot>();
        Find.Register<PlanetsGenerator> ();
        Find.Register<ModsManager> ();
        Find.Register<LuaContext> ();
        Find.Register<Sprites> ();
        Find.Register<ObjectsCreator> ();
        Find.Register<InputManager> ();
        Find.Register<MapRoot.Map> ();
        Find.Register<MapRoot.MapInteractor> ();
        Find.Register<MapRoot.MapRepresenter> ();
    }
    
}

