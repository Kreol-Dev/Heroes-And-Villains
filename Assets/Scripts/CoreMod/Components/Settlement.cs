using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using Demiurg.Core.Extensions;
using MapRoot;
using UnityEngine.UI;
using System.Collections.Generic;


namespace CoreMod
{
	[ECompName ("settlement")]
	public class Settlement : EntityComponent<SettlementSharedData>, ISlotted<SlotTile>
	{
		public void Receive (SlotTile data)
		{
			(gameObject.AddComponent<TilesComponent> ().tiles = new List<TileHandle> ()).Add (Find.Root<TilesRoot> ().MapHandle.GetHandle (data.X, data.Y));
			SharedData.layer.ObjectAdded.Dispatch (gameObject);

		}

		public override void CopyTo (GameObject go)
		{
			Settlement settlement = go.AddComponent<Settlement> ();
			settlement.Population = Population;
			settlement.Race = Race;
			settlement.SharedData = this.SharedData;
		}

		public int Population;
		public Sprite Race;

		public override void LoadFromTable (ITable table)
		{
			Population = (int)(double)table.Get ("population");
			string spriteName = "plains_people";//(string)table.Get ("race");
			Race = Find.Root<Sprites> ().GetSprite ("races", spriteName);
			SharedData = new SettlementSharedData ();

			string graphicsLayerName = (string)Find.Root<ModsManager> ().GetTable ("defines").Get ("STATIC_OBJECTS_LAYER");
			SharedData.layer = Find.Root<MapRoot.Map> ().GetLayer (graphicsLayerName) as IListMapLayer<GameObject>;

			//SharedData.layer.ObjectAdded.Dispatch (this.gameObject);

		}
	}

	public class SettlementSharedData
	{
		public IListMapLayer<GameObject> layer;
	}




	public class SettlementLayerPresenter : TileMapLayerPresenter<Settlement, GameObject, GOLayer, GOInteractor>
	{
		public override Settlement ObjectFromLayer (GameObject obj)
		{
			if (obj != null)
				return obj.GetComponent<Settlement> ();
			return null;
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
			selectPanelGO.SetActive (true);
			image.sprite = obj.Race;
			selectText.text = obj.Population.ToString ();
			selectObj = obj;
		}

		public override void HideObjectDesc (Settlement obj)
		{
			selectPanelGO.SetActive (false);
			selectObj = null;
		}

		public override void ShowObjectShortDesc (Settlement obj)
		{
			hoverPanelGO.SetActive (true);
			hoverText.text = obj.name;
			hoverObj = obj;
		}

		public override void HideObjectShortDesc (Settlement obj)
		{
			hoverPanelGO.SetActive (false);
			hoverObj = null;
		}

		public override void Update ()
		{
			if (selectObj != null)
				ShowObjectDesc (selectObj);
			if (hoverObj != null)
				ShowObjectShortDesc (hoverObj);
		}
	}

}


