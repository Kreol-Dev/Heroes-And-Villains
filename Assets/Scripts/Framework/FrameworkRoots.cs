using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;
using UIOBinding;
using System.Threading;

public class FrameworkRoots : MonoBehaviour
{

	void Awake ()
	{
		FindObjectOfType<ConsoleScript> ().Init ();
		Find.Register<PlanetsGenerator> ();
		Find.Register<ModsManager> ();
		Find.Register<LuaContext> ();
		Find.Register<Sprites> ();
		Find.Register<ObjectsCreator> ();
		Find.Register<InputManager> ();
		Find.Register<MapRoot.Map> ();
		Find.Register<MapRoot.MapInteractor> ();
		Find.Register<MapRoot.MapRepresenter> ();
		Find.Register<BindingRoot> ();
		Find.Register<StatesRoot> ();
		Find.Register<AI.AIRoot> ();
	}
}

