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
    
    public void Change_view()
    {
        g.ShowFrom(this.GetComponent<Scrollbar>().value);
       
        
    }

    public void scrolUP()
    {
        if (g.slider >= 1)
        {
            g.slider--;
            this.GetComponent<Scrollbar>().value -= 1 / g.ShownMessages.Count;
            g.ShowMessagePool();
        }

    }
    public void scrolDown()
    {
        if (g.slider < g.ShownMessages.Count-6)
        {
            g.slider++;
            this.GetComponent<Scrollbar>().value += 1 / g.ShownMessages.Count;
            g.ShowMessagePool();
            
        }

    }

}
