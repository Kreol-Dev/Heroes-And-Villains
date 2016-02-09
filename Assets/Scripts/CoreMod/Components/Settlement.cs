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
	public class Settlement : EntityComponent
	{

		public override void CopyTo (GameObject go)
		{
			Settlement settlement = go.AddComponent<Settlement> ();
			settlement.Population = Population;
			settlement.Race = Race;
		}

		public int Population;
		public Sprite Race;

		public override void LoadFromTable (ITable table)
		{
			Population = table.GetInt ("population");
			string spriteName = table.GetString ("race");
			Race = Find.Root<Sprites> ().GetSprite ("races", spriteName);

		}

		public override void PostCreate ()
		{

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


