using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class FilterPool : MonoBehaviour {
    public GameObject parent;
    public MessagePool Pool;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void awaking()
    {
        var console = GameObject.Find("MessagePool").GetComponent<MessagePool>();
        List<Category> fcat = console.Filtr;
        List<TypeMes> ftype = console.Filtr2;
        GameObject panel = GameObject.Find("Panel");

     
        for(int i=0;i<fcat.Count;i++)
        {
            GameObject f = GameObject.Instantiate(parent,new Vector3(parent.transform.position.x +90*(i+1),parent.transform.position.y,parent.transform.position.z),parent.transform.rotation) as GameObject;
            f.transform.SetParent(panel.transform);
            f.GetComponentInChildren<Text>().text= fcat[i].Name;
            f.gameObject.name = "Toggle"+i.ToString();
            f.GetComponent<Filter>().cat = fcat[i];
            f.GetComponent<Filter>().console = console;
           // GameObject.Find("MessagePool").GetComponent<MessagePool>().ActiveFilter.Add(fcat[i]);
        }

        for (int i = fcat.Count; i <fcat.Count+ ftype.Count; i++)
        {
            GameObject f = GameObject.Instantiate(parent) as GameObject;
            f.transform.SetParent(panel.transform);
            f.GetComponentInChildren<Text>().text = ftype[i-fcat.Count].Name;
            f.gameObject.name = "Toggle" + i.ToString();
            f.GetComponent<Filter>().type = ftype[i-fcat.Count];            
            f.GetComponent<Filter>().console = console;
            // GameObject.Find("MessagePool").GetComponent<MessagePool>().ActiveFilter.Add(fcat[i]);
        }
    }

}
