using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class scroling : MonoBehaviour {

    MessagePool g;
    // Use this for initialization
    void Start () {
        g = GameObject.Find("MessagePool").GetComponent<MessagePool>();
    }
	
	// Update is called once per frame
	void Update () {
        /*if (Input.GetAxis("Mouse ScrollWheel")!=0)
        {
            Scrollbar s = GameObject.Find("Scrollbar").GetComponent<Scrollbar>();
            s.value += Input.GetAxis("Mouse ScrollWheel");
            
        }*/
	
	}
    
    public void Change_view(float value)
    {
        g.ShowFrom(value);
        
    }

}
