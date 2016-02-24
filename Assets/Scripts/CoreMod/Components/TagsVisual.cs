using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CoreMod
{
	public class TagsVisual : MonoBehaviour
	{
		[SerializeField]
		List<string> tags;
		//string tagsStr;
		//Text text;
		//RectTransform thisTransform;

		public void Setup (TagsCollection collection)
		{
			tags = new List<string> ();
			collection.TagAdded += TagAdded;
			collection.TagRemoved += TagRemoved;
//            GameObject vis = new GameObject ();
			//GameObject subCanvas = Object.Instantiate<GameObject> (Resources.Load<GameObject> ("SubCanvas"));
			//subCanvas.transform.SetParent (transform);
			//thisTransform = subCanvas.transform as RectTransform;
			//text = subCanvas.GetComponentInChildren<Text> ();
			/*text = vis.AddComponent<Text> ();
            RectTransform textTransform = vis.transform as RectTransform;
             = this.transform as RectTransform;
            textTransform.SetParent (thisTransform);

            textTransform.anchorMin = Vector2.zero;
            textTransform.anchorMax = Vector2.one;
            textTransform.sizeDelta = Vector2.zero;*/

		}

		void TagAdded (Tag tag)
		{
			tags.Add (tag.Name);
//			tagsStr = string.Join (" ", tags.ToArray ());
//			text.text = tagsStr;
//			thisTransform.sizeDelta = new Vector2 (100, tags.Count * 20);
//			thisTransform.localScale = new Vector3 (1f / 50f, 1f / 20f);
		}

		void TagRemoved (Tag tag)
		{
			tags.Remove (tag.Name);
//			tagsStr = string.Join (" ", tags.ToArray ());
//			text.text = tagsStr;
		}


	}
}


