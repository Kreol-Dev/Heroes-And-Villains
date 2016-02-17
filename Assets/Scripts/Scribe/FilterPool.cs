using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class FilterPool : MonoBehaviour {
    public GameObject parent;
    public MessagePool Pool;
    public int size = 0;
    public int FilterSize=0;
	
    public void awaking()
    {
        var console = GameObject.Find("MessagePool").GetComponent<MessagePool>();
        List<Category> fcat = console.Filtr;
        List<TypeMes> ftype = console.Filtr2;
        GameObject panel = GameObject.Find("Panel");


        destroyfilter();
        for(int i=0;i<fcat.Count;i++)
        {
            GameObject f = GameObject.Instantiate(parent,new Vector3(parent.transform.position.x +90*(i+1),parent.transform.position.y,parent.transform.position.z),parent.transform.rotation) as GameObject;
            f.transform.SetParent(panel.transform);
            f.GetComponentInChildren<Text>().text= fcat[i].Name;
            f.gameObject.name = "Toggle"+i.ToString();
            f.GetComponent<Filter>().cat = fcat[i];
            f.GetComponent<Filter>().console = console;
           
        }

        for (int i = fcat.Count; i <fcat.Count+ ftype.Count; i++)
        {
            GameObject f = GameObject.Instantiate(parent) as GameObject;
            f.transform.SetParent(panel.transform);
            f.GetComponentInChildren<Text>().text = ftype[i-fcat.Count].Name;
            f.gameObject.name = "Toggle" + i.ToString();
            f.GetComponent<Filter>().type = ftype[i-fcat.Count];            
            f.GetComponent<Filter>().console = console;
            
           
        }
        size = fcat.Count + ftype.Count;
    }
    public void destroyfilter()
    {
        for(int i=0;i<size;i++)
        {
            Destroy(GameObject.Find("Toggle" + i.ToString()));
        }
        size = 0;
    }

    public void AddFilter(Category cat,TypeMes typ)
    {
        
        /*if(cat!=null)
        {
            GameObject f = GameObject.Instantiate(parent, new Vector3(parent.transform.position.x + 90 * (FilterSize + 1), parent.transform.position.y, parent.transform.position.z), parent.transform.rotation) as GameObject;
            f.transform.SetParent(panel.transform);
            f.GetComponentInChildren<Text>().text = typ.Name;
            f.gameObject.name = "Toggle" + FilterSize.ToString();
            f.GetComponent<Filter>().cat = null;
            f.GetComponent<Filter>().type = typ;
            f.GetComponent<Filter>().console = console;
            FilterSize++;
        }
        if(typ!=null)
        {
            GameObject f = GameObject.Instantiate(parent, new Vector3(parent.transform.position.x + 90 * (FilterSize + 1), parent.transform.position.y, parent.transform.position.z), parent.transform.rotation) as GameObject;
            f.transform.SetParent(panel.transform);
            f.GetComponentInChildren<Text>().text = cat.Name;
            f.gameObject.name = "Toggle" + FilterSize.ToString();
            f.GetComponent<Filter>().cat = cat;
            f.GetComponent<Filter>().type = null;
            f.GetComponent<Filter>().console = console;
            FilterSize++;
        }*/

    }

}
