using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace CoreMod
{
	public class Map : MonoBehaviour
	{
		int thisID;
		static int id = 0;
		int currentID = 0;
		public Sprite Sprite;
		public string Name;
		SpriteRenderer spriteRenderer;
		Text text;

		public void Setup ()
		{
			Sprite.texture.filterMode = FilterMode.Point;
			id++;
			thisID = id;
			Debug.LogWarningFormat ("{0} {1}", Name, thisID);
			spriteRenderer = GameObject.Find ("Map").GetComponent<SpriteRenderer> ();
			spriteRenderer.sortingLayerName = "Default";
			//spriteRenderer.gameObject.transform.localScale = new Vector3 (Sprite.texture.width, Sprite.texture.width, 1);
			text = GameObject.Find ("MapName").GetComponent<Text> ();
			if (thisID == currentID)
			{
				spriteRenderer.sprite = Sprite;
				text.text = Name;
			}
		}

		void Update ()
		{
           
			if (Input.GetKeyDown (KeyCode.DownArrow))
			{
				currentID--;
				if (currentID < 0)
					currentID = 0;
				if (thisID == currentID)
				{
					spriteRenderer.sprite = Sprite;
					text.text = Name;
				}
				Debug.Log (currentID);
				if (currentID == 0)
				{

					spriteRenderer.sprite = null;
					text.text = null;
				}
				
			}
			if (Input.GetKeyDown (KeyCode.UpArrow))
			{
				currentID++;
				if (currentID > id)
					currentID = id;
				if (thisID == currentID)
				{
					spriteRenderer.sprite = Sprite;
					text.text = Name;
				}

			}


		}
	}
}


