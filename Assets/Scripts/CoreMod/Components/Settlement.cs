using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using MapRoot;
using UnityEngine.UI;
using System.Collections.Generic;
using UIO;


namespace CoreMod
{
	[AShared]
	[ECompName ("settlement")]
	public class Settlement : EntityComponent
	{

		public override EntityComponent CopyTo (GameObject go)
		{
			Settlement settlement = go.AddComponent<Settlement> ();
			settlement.BaseFood = this.BaseFood;
			settlement.BaseProduction = this.BaseProduction;
			settlement.Food = this.Food;
			settlement.FoodMod = this.FoodMod;
			settlement.ProductionMod = this.ProductionMod;
			settlement.Population = this.Population;
			settlement.Wealth = this.Wealth;
			settlement.Race = this.Race;
			return settlement;
		}

		public int ResultProduction { get { return (int)((float)BaseProduction * ProductionMod); } }

		public int ResultFood { get { return (int)((float)BaseFood * FoodMod); } }

		[Defined ("base_production")]
		public int BaseProduction;

		[Defined ("base_food")]
		public int BaseFood;

		[Defined ("wealth")]
		public int Wealth;

		[Defined ("production")]
		public int Production;

		[Defined ("food")]
		public int Food;

		[Defined ("production_mod")]
		public float ProductionMod;

		[Defined ("food_mod")]
		public float FoodMod;

		[Defined ("population")]
		public int Population;

		public Sprite Race;

		public override void LoadFromTable (ITable table)
		{
			Find.Root<ModsManager> ().Defs.LoadObjectAs<Settlement> (this, table);
			string spriteName = table.GetString ("race");
			Race = Find.Root<Sprites> ().GetSprite ("races", spriteName);

		}

		[SerializeField]
		C_Population populationTarget;

		public override void PostCreate ()
		{
			populationTarget = new C_Population ();
			populationTarget.Setup (this);
			Find.Root<Ticker> ().Tick += OnTick;
		}


		void OnTick ()
		{
			int deltaFood = ResultFood - Population;
			int deltaProduction = ResultProduction - Population;
			Food += deltaFood;
			Production += deltaProduction;
			if (Food < 0)
			{
				Food = 0;
				Population += deltaFood;
				if (Population == 0)
				{
					gameObject.SetActive (false);
					Find.Root<Ticker> ().Tick -= OnTick;
				}
			}

			if (Production < 0)
				Production = 0;

			if (populationTarget.PlannedAction == null)
			{

				populationTarget.TargetPopulation = Population + 20;
				GetComponent<AI.Planner> ().Plan (populationTarget);
//				Debug.LogWarning (populationTarget.PlannedAction);
				populationTarget.DePlan ();
			}
			

		}

		protected override void PostDestroy ()
		{
			Find.Root<Ticker> ().Tick -= OnTick;
		}

	}

	public class ProductionState : IntState<Settlement>
	{
		public override void Add (Settlement cmp, int value)
		{
			cmp.BaseProduction += value;
		}

		public override void Mul (Settlement cmp, float value)
		{
			cmp.BaseProduction = (int)((float)cmp.BaseProduction * value);
		}

		public override int Get (Settlement cmp)
		{
			return cmp.ResultProduction;
		}

		public override void Set (Settlement cmp, int value)
		{
			cmp.BaseProduction = value;
		}
	}


	public class FoodState : IntState<Settlement>
	{
		public override void Add (Settlement cmp, int value)
		{
			cmp.BaseFood += value;
		}

		public override void Mul (Settlement cmp, float value)
		{
			cmp.BaseFood = (int)((float)cmp.BaseFood * value);
		}

		public override int Get (Settlement cmp)
		{
			return cmp.ResultFood;
		}

		public override void Set (Settlement cmp, int value)
		{
			cmp.BaseFood = value;
		}
	}



	public class SettlementPresenter : ObjectPresenter<Settlement>
	{
		Image image;
		Text hoverText;
		Text selectText;
		GameObject hoverPanelGO;
		GameObject selectPanelGO;

		Settlement hoverObj;
		Settlement selectObj;

		public override void Setup (ITable definesTable)
		{
			GameObject selectionGO = GameObject.Find ("SelectionPanel");
			GameObject hoverGO = GameObject.Find ("HoverPanel");
			hoverPanelGO = Object.Instantiate (Resources.Load ("UI/Panel"), Vector3.zero, Quaternion.identity) as GameObject; 
			hoverPanelGO.name = "Hover panel";
			hoverPanelGO.transform.SetParent (hoverGO.transform, false);
			selectPanelGO = Object.Instantiate (Resources.Load ("UI/VerticalLayoutPanel")) as GameObject; 
			hoverPanelGO.name = "Selection panel";
			selectPanelGO.transform.SetParent (selectionGO.transform, false);

			RectTransform hoverTransform = hoverPanelGO.GetComponent<RectTransform> ();
			hoverTransform.sizeDelta = Vector2.zero;
			RectTransform selectionTransform = selectPanelGO.GetComponent<RectTransform> ();
			selectionTransform.sizeDelta = Vector2.zero;
			GameObject imageGO = Object.Instantiate (Resources.Load ("UI/Image"), Vector3.zero, Quaternion.identity) as GameObject; 
			imageGO.name = "Image";
			image = imageGO.GetComponent<Image> ();
			imageGO.transform.SetParent (selectionTransform, false);


			GameObject hoverTextGO = Object.Instantiate (Resources.Load ("UI/Text"), Vector3.zero, Quaternion.identity) as GameObject; 
			hoverTextGO.transform.SetParent (hoverTransform, false);
			hoverText = hoverTextGO.GetComponent<Text> ();

			GameObject selectionTextGO = Object.Instantiate (Resources.Load ("UI/Text"), Vector3.zero, Quaternion.identity) as GameObject; 
			selectionTextGO.transform.SetParent (selectionTransform, false);
			selectText = selectionTextGO.GetComponent<Text> ();

			hoverPanelGO.SetActive (false);
			selectPanelGO.SetActive (false);
		}


		public override void ShowObjectDesc (Settlement obj)
		{
			Debug.LogWarning ("SHOW CLICK");
			selectPanelGO.SetActive (true);
			image.sprite = obj.Race;
			selectText.text = obj.Population.ToString ();
			selectObj = obj;
		}

		public override void HideObjectDesc ()
		{
			Debug.LogWarning ("HIDE CLICK");
			selectPanelGO.SetActive (false);
			selectObj = null;
		}

		public override void ShowObjectShortDesc (Settlement obj)
		{
			Debug.LogWarning ("SHOW HOVER");
			hoverPanelGO.SetActive (true);
			hoverText.text = obj.name;
			hoverObj = obj;
		}

		public override void HideObjectShortDesc ()
		{
			Debug.LogWarning ("HIDE HOVER");
			hoverPanelGO.SetActive (false);
			hoverObj = null;
		}

		void Update ()
		{
			if (selectObj != null)
				ShowObjectDesc (selectObj);
			if (hoverObj != null)
				ShowObjectShortDesc (hoverObj);
		}
	}

}


