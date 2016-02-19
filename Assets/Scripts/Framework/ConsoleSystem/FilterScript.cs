using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class FilterScript : MonoBehaviour
{

    [SerializeField]
    Toggle toggle;
    [SerializeField]
    Text text;   
    public MessagesScript messages;
    public Category cat;

    public void Set(string name, bool state)
    {
        text.text = name;
        toggle.isOn = state;
    }
    
    public void ChangeFilter()
    {
        if (this.GetComponentInChildren<Toggle>().isOn)       
            messages.ShowCategory(cat);
        else        
            messages.HideCategory(cat);
            
       
        
    }
}
