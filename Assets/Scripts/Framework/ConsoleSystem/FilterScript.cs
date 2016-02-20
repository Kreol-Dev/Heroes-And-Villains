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
    [SerializeField]
    Color custom_color;
    public MessagesScript messages;
    public Category cat;
    public MessageType type;
    public void Set(string name, bool state)
    {
        text.text = name;
        toggle.isOn = state;
    }
    
    public void ChangeFilter()
    {
        if (cat != null)
        {
            if (this.GetComponentInChildren<Toggle>().isOn)
                messages.ShowCategory(cat);
            else
                messages.HideCategory(cat);
        }
        else
        {
            if (this.GetComponentInChildren<Toggle>().isOn)
            {
                messages.ActiveType.Add(type);
                messages.FilterByType();
            }               

            else
            {
                messages.ActiveType.Remove(type);
                messages.FilterByType();               
            }
                
        }      
       
        
    }
    public void SetInActive()
    {
        if(cat!=null)
        if(this.GetComponentInChildren<Toggle>().isOn)
           this.GetComponent<Image>().color = messages.GetHash(cat.Name) ;
        else
           this.GetComponent<Image>().color = custom_color;
        else
            if (this.GetComponentInChildren<Toggle>().isOn)
            this.GetComponent<Image>().color = messages.GetHash(type.ToString());
        else
            this.GetComponent<Image>().color = custom_color;


    }
}
