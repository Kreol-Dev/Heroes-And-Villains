﻿using UnityEngine;
using System.Collections;
using MapRoot;
using UnityEngine.UI;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace CoreMod
{
	[ECompName ("encounter")]
	public class Encounter : EntityComponent
	{
		public override void PostCreate ()
		{
		}

		public string Description { get; internal set; }

		public int Danger { get; internal set; }

		public EncounterSharedData SharedData { get; internal set; }

		public override void LoadFromTable (Demiurg.Core.Extensions.ITable table)
		{
			Description = table.GetString ("description");
			Danger = table.GetInt ("danger");
			SharedData = new EncounterSharedData ();
//
//			string graphicsLayerName = Find.Root<ModsManager> ().GetTable ("defines").GetString ("STATIC_OBJECTS_LAYER");
//			SharedData.layer = Find.Root<MapRoot.Map> ().GetLayer (graphicsLayerName) as IListMapLayer<GameObject>;
		}


		public override void CopyTo (GameObject go)
		{
			Encounter e = go.AddComponent<Encounter> ();
			e.Description = this.Description;
			e.Danger = this.Danger;
			e.SharedData = this.SharedData;
		}

	}
	//public class Enco

	public class TiledEncountersLayer : GOLayer<Encounter>
	{
		
	}

	public class EncounterLayerPresenter : ComponentLayerPresenter<Encounter, TiledEncountersLayer>
	{
	}


	public class EncounterPresenter : ObjectPresenter<Encounter>
	{
		Text hoverText;
		Text selectText;
		GameObject hoverPanelGO;
		GameObject selectPanelGO;

		Encounter hoverObj;
		Encounter selectObj;

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
						
			GameObject hoverTextGO = Object.Instantiate (Resources.Load ("UI/Text"), Vector3.zero, Quaternion.identity) as GameObject; 
			hoverTextGO.transform.SetParent (hoverTransform, false);
			hoverText = hoverTextGO.GetComponent<Text> ();

			GameObject selectionTextGO = Object.Instantiate (Resources.Load ("UI/Text"), Vector3.zero, Quaternion.identity) as GameObject; 
			selectionTextGO.transform.SetParent (selectionTransform, false);
			selectText = selectionTextGO.GetComponent<Text> ();

			hoverPanelGO.SetActive (false);
			selectPanelGO.SetActive (false);
		}


		public override void ShowObjectDesc (Encounter obj)
		{
			Debug.LogWarning ("SHOW CLICK");
			selectPanelGO.SetActive (true);

			selectText.text = string.Format ("Description: {0} Danger: {1}", obj.Description, obj.Danger);
			selectObj = obj;
		}

		public override void HideObjectDesc ()
		{
			Debug.LogWarning ("HIDE CLICK");
			selectPanelGO.SetActive (false);
			selectObj = null;
		}

		public override void ShowObjectShortDesc (Encounter obj)
		{
			Debug.LogWarning ("SHOW HOVER");
			hoverPanelGO.SetActive (true);
			hoverText.text = obj.gameObject.name;
			hoverObj = obj;
		}

		public override void HideObjectShortDesc ()
		{
			Debug.LogWarning ("HIDE HOVER");
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
