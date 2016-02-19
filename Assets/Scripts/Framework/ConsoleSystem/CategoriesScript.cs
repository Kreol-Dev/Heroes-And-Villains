using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class CategoriesScript : MonoBehaviour
{
    public GameObject parent;
    [SerializeField]
    GameObject panel;
    [SerializeField]
    MessagesScript mes;
    int FilterSize = 0;


    public void AddCategory(Category c)
    {
        GameObject f = GameObject.Instantiate(parent) ;
        f.transform.SetParent(panel.transform,false);       
        f.gameObject.name = "Category" + FilterSize.ToString();
        f.GetComponent<FilterScript>().cat = c;
        f.GetComponent<FilterScript>().Set(c.Name, true);
        f.GetComponent<FilterScript>().messages = mes;
        f.GetComponent<Image>().color = mes.GetHash(c.Name);
        FilterSize++;
    }

}
