using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using MapRoot;
using UnityEngine.UI;
using System.Collections.Generic;
using UIO;
using AI;


namespace CoreMod
{
	[AShared]
	[ECompName ("settlement")]
	public class Settlement : EntityComponent
	{
		static Material spriteMaterial;

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
			settlement.CityImage = this.CityImage;
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

		public Sprite Image;
		public Sprite CityImage;
		public Sprite Race;

		public override void LoadFromTable (ITable table)
		{
			Find.Root<ModsManager> ().Defs.LoadObjectAs<Settlement> (this, table);
			string spriteName = table.GetString ("race");
			Race = Find.Root<Sprites> ().GetSprite ("races", spriteName);

			CityImage = Find.Root<Sprites> ().GetSprite ("city_strip", table.GetString ("city_image"));
			spriteMaterial = Resources.Load<Material> ("DefaultSpriteMaterial");
			//CityImage.pivot = Vector2.zero;
			//Race = Find.Root<Sprites> ().GetSprite ("races", spriteName);

		}

		[SerializeField]
		C_Population populationTarget;

		public override void PostCreate ()
		{
			populationTarget = new C_Population ();
			populationTarget.Setup (this);
			var cmp = gameObject.AddComponent<SpriteRenderer> ();
			cmp.sprite = CityImage;
			cmp.material = spriteMaterial;
		}



		protected override void PostDestroy ()
		{
		}

	}



	public class PopulationState : IntState<Settlement>
	{
		public override void Add (Settlement cmp, int value)
		{
			cmp.Population += value;
		}

		public override void Mul (Settlement cmp, float value)
		{
			cmp.Population = (int)((float)cmp.Population * value);
		}

		public override int Get (Settlement cmp)
		{
			return cmp.Population;
		}

		public override void Set (Settlement cmp, int value)
		{
			cmp.Population = value;
		}
	}

	public class WealthState : IntState<Settlement>
	{
		public override void Add (Settlement cmp, int value)
		{
			cmp.Wealth += value;
		}

		public override void Mul (Settlement cmp, float value)
		{
			cmp.Wealth = (int)((float)cmp.Wealth * value);
		}

		public override int Get (Settlement cmp)
		{
			return cmp.Wealth;
		}

		public override void Set (Settlement cmp, int value)
		{
			cmp.Wealth = value;
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

	public class FoodEffState : FloatState<Settlement>
	{
		public override float Get (Settlement cmp)
		{
			return cmp.FoodMod;
		}

		public override void Set (Settlement cmp, float value)
		{
			cmp.FoodMod = value;
		}

		public override void Add (Settlement cmp, float value)
		{
			cmp.FoodMod += value;
		}

		public override void Mul (Settlement cmp, float value)
		{
			cmp.FoodMod *= value;
		}
	}

	public class ProdEffState : FloatState<Settlement>
	{
		public override float Get (Settlement cmp)
		{
			return cmp.ProductionMod;
		}

		public override void Set (Settlement cmp, float value)
		{
			cmp.ProductionMod = value;
		}

		public override void Add (Settlement cmp, float value)
		{
			cmp.ProductionMod += value;
		}

		public override void Mul (Settlement cmp, float value)
		{
			cmp.ProductionMod *= value;
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


