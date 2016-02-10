using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Filter : MonoBehaviour {
    public Category cat;
    public MessagePool console;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChageFilter(bool isOn)
    {
        if (isOn)
            console.ShowCategory(cat);
        else
            console.HideCategory(cat);

    }
}
