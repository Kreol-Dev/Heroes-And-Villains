using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;

namespace CoreMod
{
    public class GraphicsObjectPresenter : ObjectPresenter<GraphicsTile>
    {
        Image image;
        Text hoverText;
        Text selectText;
        GameObject hoverPanelGO;
        GameObject selectPanelGO;

        GraphicsTile hoverObj;
        GraphicsTile selectObj;

        public override void Setup (ITable definesTable)
        {
            GameObject selectionGO = GameObject.Find ("SelectionPanel");
            GameObject hoverGO = GameObject.Find ("HoverPanel");
            hoverPanelGO = new GameObject ("GraphicsTileHover");
            hoverPanelGO.transform.SetParent (hoverGO.transform);
            selectPanelGO = new GameObject ("GraphicsTileSelect");
            selectPanelGO.transform.SetParent (selectionGO.transform);

            selectPanelGO.AddComponent<GridLayoutGroup> ();
            RectTransform hoverTransform = hoverPanelGO.transform as RectTransform;
            RectTransform selectionTransform = selectPanelGO.transform as RectTransform;


            GameObject imageGO = new GameObject ("Image");
            image = imageGO.AddComponent<Image> ();
            imageGO.transform.SetParent (selectionTransform, false);
            

            GameObject hoverTextGO = new GameObject ("HoverText");
            hoverTextGO.transform.SetParent (hoverTransform);
            hoverText = hoverTextGO.AddComponent<Text> ();

            GameObject selectionTextGO = new GameObject ("SelectionText");
            selectionTextGO.transform.SetParent (selectionTransform);
            selectText = selectionTextGO.AddComponent<Text> ();

            selectionGO.SetActive (false);
            hoverGO.SetActive (false);
        }


        public override void ShowObjectDesc (GraphicsTile obj)
        {
            selectPanelGO.SetActive (true);
            image.sprite = obj.Sprite;
            selectText.text = obj.Name;
            selectObj = obj;
        }

        public override void HideObjectDesc (GraphicsTile obj)
        {
            selectPanelGO.SetActive (false);
            selectObj = null;
        }

        public override void ShowObjectShortDesc (GraphicsTile obj)
        {
            hoverPanelGO.SetActive (true);
            hoverText.text = obj.Name;
            hoverObj = obj;
        }

        public override void HideObjectShortDesc (GraphicsTile obj)
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


