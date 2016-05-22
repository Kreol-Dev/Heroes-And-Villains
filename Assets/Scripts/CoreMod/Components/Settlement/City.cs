using UnityEngine;
using System.Collections;
using UIO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapRoot;
using UnityEngine.UI;
using System;

namespace CoreMod
{
	[AShared]
	[ECompName ("city")]
	public class City : EntityComponent
	{
		[SerializeField]
		List<POP> pops = new List<POP> ();
		[SerializeField]
		List<Building> buildings = new List<Building> ();
		[SerializeField]
		List<Resource> resources = new List<Resource> ();
		static POPType[] popsTypes;
		static System.Random random;
		public string Name;

		public override EntityComponent CopyTo (GameObject go)
		{
			return go.AddComponent<City> ();
		}

		public override void PostCreate ()
		{
			Find.Root<AI.Ticker> ().Tick += OnTick;
			var resTypes = Find.Root<ResourcesRoot> ().Get ();
			for (int i = 0; i < resTypes.Length; i++)
				resources.Add (new Resource (){ Cost = resTypes [i].BaseCost, Count = 0, Type = resTypes [i] });
			this.Name = Find.Root<NamesRoot> ().GenerateCityName ();
		}

		public override void LoadFromTable (ITable table)
		{
			popsTypes = Find.Root<POPsRoot> ().Get ();
			random = new System.Random ((int)Find.Root<AI.Ticker> ().TickDelta);
		}

		protected override void PostDestroy ()
		{
			Find.Root<AI.Ticker> ().Tick -= OnTick;
		}

		Resource FindRes (ResourceType type)
		{
			for (int i = 0; i < resources.Count; i++)
				if (resources [i].Type == type)
					return resources [i];
			return null;
		}

		void OnTick ()
		{
			for (int i = 0; i < popsTypes.Length; i++)
			{
				if (random.Next (100) > 90)
				{
					POPType type = popsTypes [i];
					bool enoughRes = true;
					for (int j = 0; j < type.Consume.Length; j++)
					{

						var res = type.Consume [j];
						var cityRes = FindRes (res.Type);
						if (cityRes.Count < res.Amount * 10)
						{
							enoughRes = false;
							break;
						}
					}
					if ((enoughRes || pops.Count == 0) && random.Next (100) < type.Chance)
					{
						POP pop = new POP (){ Count = 10, Happiness = 50, Type = type };
						pops.Add (pop);
					}
				}

			}
			for (int i = 0; i < pops.Count; i++)
			{
				POP pop = pops [i];
				for (int j = 0; j < pop.Type.Consume.Length; j++)
				{
					var res = pop.Type.Consume [j];
					var cityRes = FindRes (res.Type);
					cityRes.Count -= res.Amount * pop.Count;
					if (cityRes.Count < 0)
					{
						cityRes.Count = 0;
						cityRes.Cost += 0.15f;
						pop.Happiness--;
					} else
					{
						cityRes.Cost -= 0.15f;
						pop.Happiness++;
						if (cityRes.Cost < cityRes.Type.BaseCost)
							cityRes.Cost = cityRes.Type.BaseCost;
					}
				}
				if (pop.Happiness > 70)
				{
					if (random.Next (100) > pop.Happiness)
					{
						pop.Count++;
					}
				
				} else if (pop.Happiness > 30)
				{
					if (random.Next (100) > 95)
					{
						int maxValue = 0;
						for (int j = 0; j < pop.Type.Transits.Length; j++)
							maxValue += pop.Type.Transits [i].Chance;
						int transitValue = random.Next (maxValue);
						int lowerBound = 0;
						for (int j = 0; j < pop.Type.Transits.Length; j++)
						{
							int upperBound = lowerBound + pop.Type.Transits [i].Chance;
							if (transitValue >= lowerBound && transitValue < upperBound)
								pop.Type = pop.Type.Transits [j].Type;
							lowerBound = upperBound;
						}
					}

				} else
				{
					if (random.Next (100) > pop.Happiness)
					{
						pop.Count--;
						if (pop.Count == 0)
						{
							pops.RemoveAt (i);
							i--;
						}
					}

				}
				for (int j = 0; j < pop.Type.Produce.Length; j++)
				{
					var res = pop.Type.Produce [j];
					var cityRes = FindRes (res.Type);
					cityRes.Count += res.Amount * pop.Count;
				}
			}

			for (int i = 0; i < buildings.Count; i++)
			{
				Building building = buildings [i];
				bool consumedEnough = true;
				for (int j = 0; j < building.Type.Consume.Length; j++)
				{
					var res = building.Type.Consume [j];
					var cityRes = FindRes (res.Type);
					cityRes.Count -= res.Amount;
					if (cityRes.Count < 0)
						consumedEnough = false;
				}
				if (consumedEnough)
				{
					for (int j = 0; j < building.Type.Produce.Length; j++)
					{
						var res = building.Type.Produce [j];
						var cityRes = FindRes (res.Type);
						cityRes.Count += res.Amount;
					}
				} else
				{
					for (int j = 0; j < building.Type.Consume.Length; j++)
					{
						var res = building.Type.Consume [j];
						var cityRes = FindRes (res.Type);
						cityRes.Count += res.Amount;
					}
				}

			}
		}


	}


	[System.Serializable]
	public class POP
	{
		public POPType Type;
		public int Count;
		public int Happiness;
	}

	[System.Serializable]
	public class Building
	{
		public BuildingType Type;
	}

	[System.Serializable]
	public class Resource
	{
		public ResourceType Type;
		public int Count;
		public float Cost;
	}

	[System.Serializable]
	public class EconomicAgent
	{
		[System.Serializable]
		public struct Resource
		{
			public ResourceType Type;
			public int Amount;
		}


		public Resource[] Produce;
		public Resource[] Consume;

		public void LoadFromTable (ITable table, object key)
		{
			ITable agentTable = table.GetTable (key);
			var produces = agentTable.GetTable ("produces");
			var consumps = agentTable.GetTable ("consumps");
		
			var productKeys = produces.GetKeys ();
			var consumpsKeys = consumps.GetKeys ();
			var resRoot = Find.Root<ResourcesRoot> ();
			List<Resource> list = new List<Resource> ();
			foreach (var productKey in productKeys)
			{
				Resource res = new Resource ();
				res.Amount = produces.GetInt (productKey);
				res.Type = resRoot.Get ((string)productKey);
				list.Add (res);
			}
			Produce = list.ToArray ();
			list.Clear ();
			foreach (var productKey in consumpsKeys)
			{
				Resource res = new Resource ();
				res.Amount = consumps.GetInt (productKey);
				res.Type = resRoot.Get ((string)productKey);
				list.Add (res);
			}
			Consume = list.ToArray ();
			list.Clear ();
		}

	}

	[System.Serializable]
	public class POPType : EconomicAgent
	{
		public int Chance;


		public struct Transit
		{
			public POPType Type;
			public int Chance;
		}

		public Transit[] Transits;

		public string Name;
		public Sprite Sprite;


		public void LoadFromTable (ITable table, object key)
		{
			base.LoadFromTable (table, key);

			ITable popTable = table.GetTable (key);
			Chance = popTable.GetInt ("chance");
			Name = popTable.GetString ("name");
			Sprite = Find.Root<Sprites> ().GetSprite ("pops", popTable.GetString ("sprite"));

			var transit = popTable.GetTable ("transit_to");

			var popKeys = transit.GetKeys ();
			var popsRoot = Find.Root<POPsRoot> ();
			List<Transit> popsList = new List<Transit> ();
			foreach (var popKey in popKeys)
			{
				Transit t = new Transit ();
				t.Chance = transit.GetInt (popKey);
				t.Type = popsRoot.Get ((string)popKey);
				popsList.Add (t);
			}
			Transits = popsList.ToArray ();
			popsList.Clear ();
		}
	}

	[System.Serializable]
	public class BuildingType : EconomicAgent
	{
		public string Name;
		public Sprite Sprite;
		public Resource[] Cost;

		public void LoadFromTable (ITable table, object key)
		{
			base.LoadFromTable (table, key);
			ITable buildingTable = table.GetTable (key);
			var cost = buildingTable.GetTable ("cost");

			var resKeys = cost.GetKeys ();
			var resRoot = Find.Root<ResourcesRoot> ();
			List<Resource> list = new List<Resource> ();
			foreach (var resKey in resKeys)
			{
				Resource res = new Resource ();
				res.Amount = cost.GetInt (resKey);
				res.Type = resRoot.Get ((string)resKey);
				list.Add (res);
			}
			Cost = list.ToArray ();
			list.Clear ();


			Name = buildingTable.GetString ("name");
			Sprite = Find.Root<Sprites> ().GetSprite ("buildings", buildingTable.GetString ("sprite"));
		}
	}

	[System.Serializable]
	public class ResourceType
	{
		public int BaseCost;
		public string Name;
		public Sprite Sprite;

		public void LoadFromTable (ITable table, object key)
		{
			ITable resTable = table.GetTable (key);
			BaseCost = resTable.GetInt ("base_cost");
			Name = resTable.GetString ("name");
			Sprite = Find.Root<Sprites> ().GetSprite ("resources", resTable.GetString ("sprite"));
		}
	}

	[RootDependencies (typeof(ModsManager))]
	public class ResourcesRoot : ModRoot
	{
		Dictionary<string, ResourceType> types = new Dictionary<string, ResourceType> ();

		public ResourceType Get (string resName)
		{
			ResourceType type = null;
			types.TryGetValue (resName, out type);
			return type;
		}

		public ResourceType[] Get ()
		{
			
			ResourceType[] types = new ResourceType[this.types.Count];
			int i = 0;
			foreach (var type in this.types)
			{
				types [i] = type.Value;
				i++;
			}
			return types;
		}

		protected override void CustomSetup ()
		{
			var modsRoot = Find.Root<ModsManager> ();
			var resTable = modsRoot.GetTable ("resources");
			var namespaces = resTable.GetKeys ();
			foreach (var names in namespaces)
			{
				if (modsRoot.IsTechnical (resTable, names))
					continue;
				var localTable = resTable.GetTable (names);
				var keys = localTable.GetKeys ();
				foreach (var key in keys)
				{
					ResourceType type = new ResourceType ();
					type.LoadFromTable (localTable, key);
					types.Add ((string)names + "_" + (string)key, type);
				}
			}
			Fulfill.Dispatch ();
		}
	}

	[RootDependencies (typeof(ModsManager))]
	public class POPsRoot : ModRoot
	{
		Dictionary<string, POPType> types = new Dictionary<string, POPType> ();

		public POPType Get (string resName)
		{
			POPType type = null;
			types.TryGetValue (resName, out type);
			return type;
		}

		public POPType[] Get ()
		{

			POPType[] types = new POPType[this.types.Count];
			int i = 0;
			foreach (var type in this.types)
			{
				types [i] = type.Value;
				i++;
			}
			return types;
		}

		protected override void CustomSetup ()
		{
			var modsRoot = Find.Root<ModsManager> ();
			var resTable = modsRoot.GetTable ("pops");
			var namespaces = resTable.GetKeys ();
			foreach (var names in namespaces)
			{
				if (modsRoot.IsTechnical (resTable, names))
					continue;
				var localTable = resTable.GetTable (names);
				var keys = localTable.GetKeys ();
				foreach (var key in keys)
				{
					POPType type = new POPType ();
					types.Add ((string)names + "_" + (string)key, type);
				}
			}

			Fulfill.Dispatch ();
			foreach (var names in namespaces)
			{
				if (modsRoot.IsTechnical (resTable, names))
					continue;
				var localTable = resTable.GetTable (names);
				var keys = localTable.GetKeys ();
				foreach (var key in keys)
				{
					types [(string)names + "_" + (string)key].LoadFromTable (localTable, key);
				}
			}

		}
	}

	[RootDependencies (typeof(ModsManager))]
	public class BuildingsRoot : ModRoot
	{
		Dictionary<string, BuildingType> types = new Dictionary<string, BuildingType> ();

		public BuildingType[] Get ()
		{

			BuildingType[] types = new BuildingType[this.types.Count];
			int i = 0;
			foreach (var type in this.types)
			{
				types [i] = type.Value;
				i++;
			}
			return types;
		}

		public BuildingType Get (string resName)
		{
			BuildingType type = null;
			types.TryGetValue (resName, out type);
			return type;
		}

		protected override void CustomSetup ()
		{
			var modsRoot = Find.Root<ModsManager> ();
			var resTable = modsRoot.GetTable ("buildings");
			var namespaces = resTable.GetKeys ();
			foreach (var names in namespaces)
			{
				if (modsRoot.IsTechnical (resTable, names))
					continue;
				var localTable = resTable.GetTable (names);
				var keys = localTable.GetKeys ();
				foreach (var key in keys)
				{
					BuildingType type = new BuildingType ();
					type.LoadFromTable (localTable, key);
					types.Add ((string)names + "_" + (string)key, type);
				}
			}

			Fulfill.Dispatch ();
		}
	}

	public class NamesRoot : ModRoot
	{
		System.Random random;
		StringBuilder nameBuilder = new StringBuilder ();

		public string GenerateBiomeName ()
		{
			string sem1 = semantics [random.Next (semantics.Count)];
			string sem2 = null;
			if (random.Next (100) > 30)
				sem2 = semantics [random.Next (adjectives.Count)];
			string adjective = null;
			if (random.Next (100) > 30)
				adjective = adjectives [random.Next (adjectives.Count)];

			nameBuilder.Length = 0;
			if (adjective != null)
			{
				nameBuilder.Append (adjective);
				nameBuilder.Append (" ");
			}
			nameBuilder.Append (sem1);
			if (sem2 != null)
			{
				int mid = random.Next (3);
				switch (mid)
				{
				case 0:
					nameBuilder.Append ("'o'");
					nameBuilder.Append (sem2);
					break;
				case 1:
					nameBuilder.Append (Char.ToLower (sem2 [0]));
					nameBuilder.Append (sem2.Substring (1));
					break;
				case 2:
					nameBuilder.Append ("'");
					nameBuilder.Append (sem2);
					break;
				case 3:
					nameBuilder.Append ("-");
					nameBuilder.Append (Char.ToLower (sem2 [0]));
					nameBuilder.Append (sem2.Substring (1));
					break;
				}
			}
			return nameBuilder.ToString ();
		}

		public string GenerateCityName ()
		{
			string adjective = null;
			if (random.Next (100) > 30)
				adjective = adjectives [random.Next (adjectives.Count)];
			string semantic = semantics [random.Next (semantics.Count)];
			string type = types [random.Next (types.Count)];
			nameBuilder.Length = 0;
			if (adjective != null)
			{
				nameBuilder.Append (adjective);
				nameBuilder.Append (" ");
			}
			nameBuilder.Append (semantic);
			nameBuilder.Append (type);

			return nameBuilder.ToString ();

		}

		List<string> adjectives = new List<string> ();
		List<string> semantics = new List<string> ();
		List<string> types = new List<string> ();

		protected override void CustomSetup ()
		{
			var namesTable = Find.Root<ModsManager> ().GetTable ("names").GetTable ("cities");
			random = new System.Random (Find.Root<ModsManager> ().GetTable ("defines").GetInt ("SEED"));
			var adjectivesTable = namesTable.GetTable ("adjectives");
			var semantic = namesTable.GetTable ("semantic");
			var types = namesTable.GetTable ("types");

			foreach (var key in adjectivesTable.GetKeys())
			{
				adjectives.Add (adjectivesTable.GetString (key));
			}
			foreach (var key in semantic.GetKeys())
			{
				semantics.Add (semantic.GetString (key));
			}
			foreach (var key in types.GetKeys())
			{
				this.types.Add (types.GetString (key));
			}
			Fulfill.Dispatch ();
		}
	}

	public class CityPresenter : ObjectPresenter<GameObject>
	{
		Text selectionText;
		Text hoverText;

		public override void Setup (ITable definesTable)
		{
			selectionText = (UnityEngine.Object.Instantiate (Resources.Load ("UI/Text")) as GameObject).GetComponent<Text> ();
			var selectionLayout = selectionText.gameObject.AddComponent<LayoutElement> ();
			selectionLayout.minHeight = 20;
			selectionLayout.minWidth = 200;
			hoverText = (UnityEngine.Object.Instantiate (Resources.Load ("UI/Text")) as GameObject).GetComponent<Text> ();
			var hoverLayout = hoverText.gameObject.AddComponent<LayoutElement> ();
			hoverLayout.minHeight = 20;
			hoverLayout.minWidth = 200;
			hoverText.text = "";
			hoverText.gameObject.SetActive (false);
			selectionText.text = "";
			selectionText.gameObject.SetActive (false);
			Transform selectionGO = GameObject.Find ("SelectionPanel").transform;
			Transform hoverGO = GameObject.Find ("HoverPanel").transform;
			selectionText.transform.SetParent (selectionGO);
			hoverText.transform.SetParent (hoverGO);
		}

		public override void ShowObjectDesc (GameObject obj)
		{
			selectionText.gameObject.SetActive (true);
			selectionText.text = obj.GetComponent<City> ().Name;
		}

		public override void HideObjectDesc ()
		{
			selectionText.gameObject.SetActive (false);
		}

		public override void ShowObjectShortDesc (GameObject obj)
		{
			hoverText.gameObject.SetActive (true);
			hoverText.text = obj.GetComponent<City> ().Name;
		}

		public override void HideObjectShortDesc ()
		{
			hoverText.gameObject.SetActive (false);
		}
	}
}

