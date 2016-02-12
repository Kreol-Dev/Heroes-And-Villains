using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Filter : MonoBehaviour {
    public Category cat;
    public TypeMes type;
    public MessagePool console ;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChageFilter()
    {
        if (this.GetComponent<Toggle>().isOn)
        {
            if (cat != null)
                console.ShowCategory(cat);
            if (type != null)
            {
                console.activeTypes[type.ID] = true;
                console.FilterByType();
            }
        }
        else
        {
            if(cat!=null)
                console.HideCategory(cat);
            if (type != null)
            {
                console.activeTypes[type.ID] = false;
                console.FilterByType();
            }
        }

    }
}
